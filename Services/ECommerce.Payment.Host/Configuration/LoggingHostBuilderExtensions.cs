using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace ECommerce.Payment.Host.Configuration
{
    static class LoggingHostBuilderExtensions
    {
        public static IHostBuilder AddLog4Net(this IHostBuilder builder, string fileName)
        {
            var log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead(fileName));
            var repo = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

            return builder.ConfigureLogging((hostContext, configLogging) =>
            {
                configLogging
                    .AddLog4Net()
                    .SetMinimumLevel(LogLevel.Debug)
                    .AddFilter("Microsoft", LogLevel.Information)
                    .AddFilter("System", LogLevel.Information);
            }); ;
        }
    }
}
