using System;
using Autofac;
using ECommerce.Payment.Host.Consumers;

namespace ECommerce.Payment.Host.Modules
{
    public class ConsumerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OrderSubmittedEventConsumer>();
        }
    }
}
