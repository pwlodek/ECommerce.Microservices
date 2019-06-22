using System;
using Autofac;
using ECommerce.Common;
using ECommerce.Common.Commands;
using ECommerce.Shipping.Host.Configuration;
using ECommerce.Shipping.Host.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

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
                    var config = context.Resolve<IConfiguration>();
                    var rabbitHost = config["Brokers:RabbitMQ:Host"];
                    var username = config["Brokers:RabbitMQ:Username"];
                    var password = config["Brokers:RabbitMQ:Password"];
                    var host = cfg.Host(new Uri($"rabbitmq://{rabbitHost}"), h =>
                    {
                        h.Username(username);
                        h.Password(password);
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
                    
                    EndpointConvention.Map<ShipOrderCommand>(new Uri($"rabbitmq://{rabbitHost}/shipping_order"));
                    EndpointConvention.Map<InitiateOrderPackingCommand>(new Uri($"rabbitmq://{rabbitHost}/shipping_order"));
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
