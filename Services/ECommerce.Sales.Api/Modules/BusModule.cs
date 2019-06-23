using System;
using Autofac;
using ECommerce.Common;
using ECommerce.Common.Commands;
using ECommerce.Sales.Api.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Sales.Api.Modules
{
    internal class BusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
            {
                var config = context.Resolve<IConfiguration>();
                var rabbitHost = config["Brokers:RabbitMQ:Host"];
                var username = config["Brokers:RabbitMQ:Username"];
                var password = config["Brokers:RabbitMQ:Password"];
                var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri($"rabbitmq://{rabbitHost}"), h =>
                    {
                        h.Username(username);
                        h.Password(password);
                    });

                    cfg.ReceiveEndpoint(host, "sales_fanout", e =>
                    {
                        e.Consumer<OrderCompletedEventConsumer>(context);
                        e.Consumer<OrderPackedEventConsumer>(context);
                        e.Consumer<PaymentAcceptedEventConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(host, "sales_submit_orders", e =>
                    {
                        e.Consumer<SubmitOrderCommandConsumer>(context);
                    });

                    EndpointConvention.Map<SubmitOrderCommand>(new Uri($"rabbitmq://{rabbitHost}/sales_submit_orders"));

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
