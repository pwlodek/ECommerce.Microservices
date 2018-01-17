using System;
using Autofac;
using ECommerce.Shipping.Host.Consumers;
using ECommerce.Shipping.Host.Sagas;
using MassTransit;
using MassTransit.Saga;

namespace ECommerce.Shipping.Host.Modules
{
    public class ConsumerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Consumers
            builder.RegisterType<InitiateOrderPackingCommandConsumer>();
            builder.RegisterType<ShipOrderCommandConsumer>();

            // Sagas
            builder.RegisterStateMachineSagas(typeof(BusModule).Assembly);
            builder.RegisterType<InMemorySagaRepository<Shipment>>().As<ISagaRepository<Shipment>>();
        }
    }
}
