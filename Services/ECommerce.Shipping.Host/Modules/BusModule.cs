using System;
using Autofac;
using ECommerce.Common;
using ECommerce.Common.Commands;
using ECommerce.Shipping.Host.Consumers;
using MassTransit;

namespace ECommerce.Shipping.Host.Modules
{
    internal class BusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
            {
                var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri($"rabbitmq://{Configuration.RabbitMqHost}"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    // https://stackoverflow.com/questions/39573721/disable-round-robin-pattern-and-use-fanout-on-masstransit
                    cfg.ReceiveEndpoint(host, "ecommerce_main_fanout" + Guid.NewGuid().ToString(), e =>
                    {
                    });

                    cfg.ReceiveEndpoint(host, "shipping_fanout", e =>
                    {
                        e.LoadStateMachineSagas(context);
                    });

                    cfg.ReceiveEndpoint(host, "shipping_order", e =>
                    {
                        e.Consumer<ShipOrderCommandConsumer>(context);
                        e.Consumer<InitiateOrderPackingCommandConsumer>(context);
                    });
                    
                    EndpointConvention.Map<ShipOrderCommand>(new Uri($"rabbitmq://{Configuration.RabbitMqHost}/shipping_order"));
                    EndpointConvention.Map<InitiateOrderPackingCommand>(new Uri($"rabbitmq://{Configuration.RabbitMqHost}/shipping_order"));
                });

                return busControl;
            })
            .SingleInstance()
            .As<IPublishEndpoint>()
            .As<IBusControl>()
            .As<IBus>();
        }
    }
}
