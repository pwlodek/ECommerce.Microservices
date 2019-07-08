using ECommerce.Common.Infrastructure.Messaging;
using ECommerce.Payment.Host.Configuration;
using ECommerce.Services.Common.Configuration;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace ECommerce.Payment.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
               .MinimumLevel.Override("System", LogEventLevel.Warning)
               .Enrich.FromLogContext()
               .WriteTo.Console()
               .WriteTo.ApplicationInsights(TelemetryConfiguration.Active, TelemetryConverter.Traces)
               .CreateLogger();

            try
            {
                Log.Information("Starting host");
                BuildHost(args).Run();
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHost BuildHost(string[] args) =>
            new HostBuilder()
                .UseConsoleLifetime()
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
                    builder.AddCloud();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IMessageCorrelationContextAccessor, MessageCorrelationContextAccessor>();
                    services.AddApplicationInsightsTelemetry(hostContext.Configuration);
                    services.AddHostedService<PaymentService>();
                })
                .UseSerilog()
                .Build();
    }
}
