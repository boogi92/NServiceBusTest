using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LimeTest.Messages.People;
using LimeTest.Messages.Poems;
using LimeTest.Messages.Reports;
using NServiceBus;

namespace LimeTest.Reports
{
    public class ReportHandler : IHandleMessages<GetReport>
    {
        public Task Handle(GetReport message, IMessageHandlerContext context)
        {

            try
            {
                Console.WriteLine("Start GetReport");
                var dirInfo = new DirectoryInfo(ConfigurationManager.AppSettings["Path"]);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                var path = $"{ConfigurationManager.AppSettings["Path"]}\\Lime_{DateTime.Now:d}.xlsx";
                var poems = Program.EndpointInstance.Request<ResponseGetPoems>(new GetPoems()).Result;
                var peoples = Program.EndpointInstance.Request<ResponseGetPeoples>(new GetPeoples()).Result;

                var dataList = peoples.Peoples.Join(poems.Poems, x => x.Id, y => y.People_Id, (x, y) => new PeopleReport
                {
                    Title = y.Title,
                    Author = y.Author,
                    Content = y.Content,
                    Distance = y.Distance,
                    Url = y.Url,
                    Email = x.Email,
                    Gender = x.Gender,
                    LastName = x.LastName,
                    Quote = x.Quote,
                    FirstName = x.FirstName,
                    PictureMedium = x.PictureMedium,
                    Address = x.Address
                });

                ReportExcel<PeopleReport>.SaveAs(path, dataList);

                
                return context.Reply(new ResponseGetReport
                {
                    Path = path
                });
            }
            catch (Exception e)
            {
                return context.Reply(new ResponseGetReport
                {
                    Error = e
                });
            }
            finally
            {
                Console.WriteLine("End GetReport");
            }
        }
    }
}