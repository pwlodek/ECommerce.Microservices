using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Services.Common.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ECommerce.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
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
                       // This is to demonstrate multiple web apps
                       o.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(1);
                   });
    }
}
