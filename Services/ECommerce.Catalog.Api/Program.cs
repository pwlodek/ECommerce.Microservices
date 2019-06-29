using System;
using System.IO;
using System.Reflection;
using System.Xml;
using ECommerce.Services.Common.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Catalog.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
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
                   .UseKestrel(o =>
                   {
                       o.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(1);
                   })
                   .Build();
    }


}
