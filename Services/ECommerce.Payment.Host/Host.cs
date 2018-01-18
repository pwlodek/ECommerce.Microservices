using System;
using System.Threading;
using Autofac;
using ECommerce.Common;
using ECommerce.Payment.Host.Modules;
using ECommerce.Services.Common.Configuration;
using MassTransit;

namespace ECommerce.Payment.Host
{
    public class Host
    {
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

            Console.WriteLine("Running Payment microservice.");
            Thread.Sleep(int.MaxValue);
        }
    }
}
