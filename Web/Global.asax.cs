using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LimeTest.Messages.People;
using LimeTest.Messages.Poems;
using LimeTest.Messages.Reports;
using NServiceBus;

namespace Web
{
    public class MvcApplication : HttpApplication
    {
        //protected void Application_Start()
        //{
        //    AreaRegistration.RegisterAllAreas();
        //    FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        //    RouteConfig.RegisterRoutes(RouteTable.Routes);
        //    BundleConfig.RegisterBundles(BundleTable.Bundles);
        //}

        IEndpointInstance _endpointInstance;

        protected void Application_Start()
        {
            AsyncStart().GetAwaiter().GetResult();
        }

        async Task AsyncStart()
        {
            var endpointConfiguration = new EndpointConfiguration("WEB");
            endpointConfiguration.MakeInstanceUniquelyAddressable("WEB");
            endpointConfiguration.EnableCallbacks();
            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(GetPeoples), "People");
            routing.RouteToEndpoint(typeof(GetInfo), "People");

            routing.RouteToEndpoint(typeof(GetPoems), "Poems");

            routing.RouteToEndpoint(typeof(GetReport), "Report");

            _endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ControllerBuilder.Current.SetControllerFactory(new InjectEndpointInstanceIntoController(_endpointInstance));
        }
    }
}
