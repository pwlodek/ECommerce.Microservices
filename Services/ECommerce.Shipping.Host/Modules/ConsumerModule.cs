using System;
using Autofac;
using ECommerce.Shipping.Host.Consumers;

namespace ECommerce.Shipping.Host.Modules
{
    public class ConsumerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InitiateOrderPackingCommandConsumer>();
            builder.RegisterType<ShipOrderCommandConsumer>();
        }
    }
}
