using Autofac;
using Autofac.Extensions.DependencyInjection;
using ECommerce.Shipping.Host.Modules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ECommerce.Shipping.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(c => c.SetMinimumLevel(LogLevel.Debug));
                    services.AddScoped<IHostedService, ShippingService>();
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddLog4Net();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory(builder => 
                {
                    builder.RegisterModule<BusModule>();
                    builder.RegisterModule<ConsumerModule>();
                }))
                .UseConsoleLifetime()
                .Build();

            host.Run();
        }
    }
}
