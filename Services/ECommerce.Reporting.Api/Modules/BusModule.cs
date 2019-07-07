using System;
using Autofac;
using ECommerce.Common.Infrastructure.Messaging;
using ECommerce.Reporting.Api.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Reporting.Api.Modules
{
    internal class BusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MessageCorrelationContextAccessor>().SingleInstance().As<IMessageCorrelationContextAccessor>();
            builder.Register(context =>
            {
                var correlationContextAccessor = context.Resolve<IMessageCorrelationContextAccessor>();
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

                    cfg.UseCorrelationId(correlationContextAccessor);

                    cfg.ReceiveEndpoint(host, "reporting_fanout", e =>
                    {
                        e.Consumer<OrderSubmittedEventConsumer>(context);
                    });
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
