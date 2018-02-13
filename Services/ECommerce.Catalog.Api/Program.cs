using System;
using System.IO;
using System.Reflection;
using System.Xml;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ECommerce.Catalog.Api
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
                    .UseStartup<Startup>()
                   .UseKestrel(o =>
                   {
                       o.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(1);
                   })
                   .Build();
    }


}
