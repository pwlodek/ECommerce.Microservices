using ECommerce.Services.Common.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using System.Xml;

namespace ECommerce.Sales.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ConfigureLogging();
            BuildWebHost(args).Run();
        }

        private static void ConfigureLogging()
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("log4net.config"));
            var repo = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
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
                   .Build();
    }
}
