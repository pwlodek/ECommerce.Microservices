using System;
using Autofac;
using ECommerce.Sales.Api.Consumers;

namespace ECommerce.Sales.Api.Modules
{
    public class ConsumerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SubmitOrderCommandConsumer>();
            builder.RegisterType<OrderCompletedEventConsumer>();
        }
    }
}
