using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace LimeTest.People
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "People";

            var endpointConfiguration = new EndpointConfiguration("People");
            endpointConfiguration.MakeInstanceUniquelyAddressable("People");
            endpointConfiguration.EnableCallbacks();

            endpointConfiguration.UseTransport<LearningTransport>();

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
