using ECommerce.Payment.Host.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
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
                .ConfigureHostConfiguration(builder => 
                {
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
                    services.AddHostedService<PaymentService>();
                })                
                .AddLog4Net("log4net.config")
                .Build()
                .Run();
        }
    }
}
