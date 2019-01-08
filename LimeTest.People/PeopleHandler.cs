using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LimeTest.Data;
using LimeTest.Messages.People;
using LimeTest.Messages.Poems;
using LimeTest.People.Model;
using Newtonsoft.Json;
using NServiceBus;

namespace LimeTest.People
{
    public class PeopleHandler : IHandleMessages<GetInfo>, IHandleMessages<GetPeoples>
    {
        public async Task Handle(GetInfo message, IMessageHandlerContext context)
        {
           
            try
            {
                Console.WriteLine("Start GetInfo");

                using (var client = new HttpClient())
                using (var db = new LimeContext())
                {
                    var response = await client.GetAsync("https://randomuser.me/api/");
                    var json = await response.Content.ReadAsStringAsync();
                    var userPeople = JsonConvert.DeserializeObject<UserPeople>(json).Results;

                    if (userPeople.Count > 0)
                    {
                        var user = userPeople[0];
                        var people = new Data.Entity.People
                        {
                            Address = $"{user.Location.City} {user.Location.Street}",
                            Email = user.Email,
                            Gender = user.Gender,
                            PictureMedium = user.Picture.Medium,
                            LastName = user.Name.Last,
                            FirstName = user.Name.First,
                        };

                        db.Peoples.Add(people);
                        db.SaveChanges();

                        response = await client.GetAsync("https://geek-jokes.sameerkumar.website/api");
                        json = await response.Content.ReadAsStringAsync();

                        people.Quote = json;
                        db.Peoples.AddOrUpdate(people);
                        db.SaveChanges();

                        var command = new GetPoem {PeopleId = people.Id};
                        await context.Publish(command).ConfigureAwait(false);

                        
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("End GetInfo");
            }
        }

        public Task Handle(GetPeoples message, IMessageHandlerContext context)
        {
            Console.WriteLine("Start GetPeoples");

            try
            {
                using (var db = new LimeContext())
                {
                    var peoples = db.Peoples.ToList();
                    var response = new ResponseGetPeoples()
                    {
                        Peoples = peoples
                    };

                    return context.Reply(response);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var response = new ResponseGetPeoples()
                {
                    Error = e
                };
                return context.Reply(response);
            }
            finally
            {
                Console.WriteLine("End GetPeoples");
            }
        }
    }
}