using System;
using System.Threading.Tasks;
using NServiceBus;

namespace LimeTest.Poems
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "Poems";

            var endpointConfiguration = new EndpointConfiguration("Poems");

            endpointConfiguration.MakeInstanceUniquelyAddressable("Poems");
            endpointConfiguration.EnableCallbacks();
            endpointConfiguration.UseTransport<LearningTransport>();

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}
