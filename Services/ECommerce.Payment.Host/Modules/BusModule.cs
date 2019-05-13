using Autofac;
using ECommerce.Common.Commands;
using ECommerce.Payment.Host.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using System;

namespace ECommerce.Payment.Host.Modules
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
                    var rabbitHost = config["RabbitHost"];

                    var host = cfg.Host(new Uri($"rabbitmq://{rabbitHost}"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    // https://stackoverflow.com/questions/39573721/disable-round-robin-pattern-and-use-fanout-on-masstransit
                    cfg.ReceiveEndpoint(host, "ecommerce_main_fanout" + Guid.NewGuid().ToString(), e =>
                    {
                    });

                    cfg.ReceiveEndpoint(host, "payment_fanout", e =>
                    {
                        e.Consumer<OrderSubmittedEventConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(host, "payment_initiate_payment", e =>
                    {
                        e.Consumer<InitiatePaymentCommandConsumer>(context);
                    });

                    EndpointConvention.Map<InitiatePaymentCommand>(new Uri($"rabbitmq://{rabbitHost}/payment_initiate_payment"));
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
