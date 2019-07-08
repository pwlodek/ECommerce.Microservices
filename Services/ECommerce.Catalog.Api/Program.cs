using System;
using System.IO;
using System.Reflection;
using System.Xml;
using ECommerce.Services.Common.Configuration;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace ECommerce.Catalog.Api
{
    public class Program
    {
        public static void Main(string[] args)
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
                Log.Information("Starting web host");
                BuildWebHost(args).Run();
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

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .ConfigureAppConfiguration((context, builder) =>
                   {
                       var orchestrator = context.Configuration["ORCHESTRATOR"];
                       builder.SetBasePath(Directory.GetCurrentDirectory());
                       builder.AddJsonFile($"appsettings.json", optional: false);
                       builder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: false);
                       builder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.{orchestrator}.json", optional: true);
                       builder.AddEnvironmentVariables();
                       builder.AddCloud();
                   })
                   .UseStartup<Startup>()
                   .UseSerilog()
                   .UseKestrel(o =>
                   {
                       o.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(1);
                   })
                   .Build();
    }
}
