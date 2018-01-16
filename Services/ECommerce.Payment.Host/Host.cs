using System;
using System.Threading;
using Autofac;
using ECommerce.Payment.Host.Modules;
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
            var builder = new ContainerBuilder();

            builder.RegisterModule<BusModule>();
            builder.RegisterModule<ConsumerModule>();

            var container = builder.Build();
            var bus = container.Resolve<IBusControl>();
            bus.Start();

            Thread.Sleep(int.MaxValue);
        }
    }
}
