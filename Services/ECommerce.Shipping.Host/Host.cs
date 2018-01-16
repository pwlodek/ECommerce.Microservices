using System;
using System.Threading;
using Autofac;
using ECommerce.Shipping.Host.Modules;
using MassTransit;

namespace ECommerce.Shipping.Host
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
            //builder.RegisterModule<ConsumerModule>();

            var container = builder.Build();
            var bus = container.Resolve<IBusControl>();
            bus.Start();

            Thread.Sleep(int.MaxValue);
        }
    }
}
