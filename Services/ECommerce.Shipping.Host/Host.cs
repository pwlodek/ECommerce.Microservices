using System;
using System.Threading;
using Autofac;
using ECommerce.Common;
using ECommerce.Services.Common.Configuration;
using ECommerce.Shipping.Host.Modules;
using log4net;
using MassTransit;

namespace ECommerce.Shipping.Host
{
    public class Host
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Host));

        public Host()
        {
        }

        public void Run()
        {
            
            var waiter = new DependencyAwaiter();
            waiter.WaitForRabbit(Configuration.RabbitMqHost);

            var builder = new ContainerBuilder();

            builder.RegisterModule<BusModule>();
            builder.RegisterModule<ConsumerModule>();

            var container = builder.Build();
            var bus = container.Resolve<IBusControl>();
            bus.Start();

            Logger.Info("Running Shipping microservice.");
            Thread.Sleep(int.MaxValue);
        }
    }
}
