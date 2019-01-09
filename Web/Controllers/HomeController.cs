using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LimeTest.Messages.People;
using LimeTest.Messages.Poems;
using LimeTest.Messages.Reports;
using NServiceBus;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        readonly IEndpointInstance _endpointInstance;

        public HomeController(IEndpointInstance endpointInstance)
        {
            _endpointInstance = endpointInstance;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<PartialViewResult> Grid()
        {
            try
            {
                var poems = await _endpointInstance.Request<ResponseGetPoems>(new GetPoems()).ConfigureAwait(false);
                var peoples = await _endpointInstance.Request<ResponseGetPeoples>(new GetPeoples()).ConfigureAwait(false);
                var data = poems.Poems.Join(peoples.Peoples, x => x.People_Id, y => y.Id, (x, y) => new PeopleModel
                {
                    Id = y.Id,
                    Gender = y.Gender,
                    FirstName = y.FirstName,
                    LastName = y.LastName,
                    Address = y.Address,
                    Email = y.Email,
                    Picture = y.PictureMedium,
                    Content = x.Content,
                    Distance = x.Distance
                });

                return PartialView("Grid", data);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public async Task<ActionResult> Generate()
        {
            var command = new GetInfo();
            await _endpointInstance.Send(command).ConfigureAwait(false);

            dynamic model = new ExpandoObject();

            return Json(model);
        }

        public async Task<ActionResult> Download()
        {
            var message = new GetReport();

            var response = await _endpointInstance.Request<ResponseGetReport>(message).ConfigureAwait(false);
            var fileBytes = System.IO.File.ReadAllBytes(response.Path);

            return File(fileBytes, "application/vnd.ms-excel", $"{Path.GetFileName(response.Path)}");
        }
    }
}