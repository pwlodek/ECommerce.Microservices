using Autofac;
using Autofac.Extensions.DependencyInjection;
using ECommerce.Shipping.Host.Configuration;
using ECommerce.Shipping.Host.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace ECommerce.Shipping.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(builder =>
                {
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                    builder.AddEnvironmentVariables(prefix: "ASPNETCORE_");
                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                    builder.AddJsonFile($"appsettings.json", optional: false);
                    builder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: false);
                    builder.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(c => c.SetMinimumLevel(LogLevel.Debug));
                    services.AddScoped<IHostedService, ShippingService>();
                    services.Configure<ServiceConfigOptions>(hostContext.Configuration);
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
