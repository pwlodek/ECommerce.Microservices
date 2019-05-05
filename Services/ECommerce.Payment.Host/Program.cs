using ECommerce.Payment.Host.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace ECommerce.Payment.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new HostBuilder();

            host.UseConsoleLifetime()
                .UseServiceProviderFactory(new DependencyProvider())
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("appsettings.json", optional: false);
                    configHost.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<PaymentService>();
                })                
                .AddLog4Net("log4net.config")
                .Build()
                .Run();
        }
    }
}
