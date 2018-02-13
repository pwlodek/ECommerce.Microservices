using System;
using Autofac;
using ECommerce.Reporting.Api.Consumers;

namespace ECommerce.Reporting.Api.Modules
{
    public class ConsumerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Consumers
            builder.RegisterType<OrderSubmittedEventConsumer>();
        }
    }
}
