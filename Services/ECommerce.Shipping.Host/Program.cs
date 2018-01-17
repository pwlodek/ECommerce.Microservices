using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Shipping.Host
{
    class Program
    {
        // AutoResetEvent to signal when to exit the application.
        private static readonly AutoResetEvent waitHandle = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            var host = new Host();

            // Fire and forget
            Task.Run(() => host.Run());

            // Handle Control+C or Control+Break
            Console.CancelKeyPress += (o, e) =>
            {
                Console.WriteLine("Exit");

                // Allow the manin thread to continue and exit...
                waitHandle.Set();
            };

            Console.WriteLine("Running Shipping microservice.");

            // Wait
            waitHandle.WaitOne();
        }
    }
}
