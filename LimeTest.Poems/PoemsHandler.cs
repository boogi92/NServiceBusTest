using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LimeTest.Data;
using LimeTest.Data.Entity;
using LimeTest.Messages.Poems;
using Newtonsoft.Json;
using NServiceBus;
using Poems.Model;
using Poems.Poems;

namespace LimeTest.Poems
{
    public class PoemsHandler : IHandleMessages<GetPoem>, IHandleMessages<GetPoems>

    {
        public async Task Handle(GetPoem message, IMessageHandlerContext context)
        {
            try
            {
                Console.WriteLine("Start GetPoem");

                using (var client = new HttpClient())
                using (var db = new LimeContext())
                {
                    var response = await client.GetAsync("https://www.poemist.com/api/v1/randompoems");
                    var json = await response.Content.ReadAsStringAsync();
                    var poems = JsonConvert.DeserializeObject<List<PoemsModel>>(json);

                    if (poems.Count > 0)
                    {

                        var poem = new Poem
                        {
                            Author = poems[0].Poet.Name,
                            Content = poems[0].Content,
                            Title = poems[0].Title,
                            Url = poems[0].Url,
                            People_Id =  message.PeopleId
                        };

                        var sentences = poems[0].Content.Split('.');
                        var number = 0;
                        double distance = 0;
                        while (number != sentences.Length - 1 && !string.IsNullOrEmpty(sentences[number + 1]))
                        {
                            distance = distance + JaroWinkler.RateSimilarity(sentences[number], sentences[++number]);
                        }

                        distance = distance != 0 && number != 0 ? distance / number : 0;

                        poem.Distance = distance;
                        db.Poems.Add(poem);
                        await db.SaveChangesAsync(); 
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("End GetPoem");
            }
        }

        public Task Handle(GetPoems message, IMessageHandlerContext context)
        {
            try
            {
                using (var db = new LimeContext())
                {
                    Console.WriteLine("Start GetPoems");

                    var poems = db.Poems.ToList();

                    var response = new ResponseGetPoems
                    {
                        Poems = poems
                    };
                    
                    return context.Reply(response);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return context.Reply(new ResponseGetPoems
                {
                    Error = e
                });
            }
            finally
            {
                Console.WriteLine("End GetPoems");
            }
        }
    }
}