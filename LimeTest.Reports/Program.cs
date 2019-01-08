using System;
using System.Threading.Tasks;
using LimeTest.Messages.People;
using LimeTest.Messages.Poems;
using LimeTest.Messages.Reports;
using NServiceBus;

namespace LimeTest.Reports
{
    public static class Program
    {
        public static IEndpointInstance EndpointInstance;

        static async Task Main()
        {
            Console.Title = "Reports";

            var endpointConfiguration = new EndpointConfiguration("Report");

            endpointConfiguration.MakeInstanceUniquelyAddressable("Report");
            endpointConfiguration.EnableCallbacks();
            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(GetPeoples), "People");
            routing.RouteToEndpoint(typeof(GetPoems).Assembly, "Poems");

            EndpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);



            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await EndpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
