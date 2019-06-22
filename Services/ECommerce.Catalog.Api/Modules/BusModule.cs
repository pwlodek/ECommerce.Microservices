using System;
using Autofac;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Catalog.Api.Modules
{
    internal class BusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
            {
                var config = context.Resolve<IConfiguration>();
                var host = config["Brokers:RabbitMQ:Host"];
                var username = config["Brokers:RabbitMQ:Username"];
                var password = config["Brokers:RabbitMQ:Password"];
                var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host(new Uri($"rabbitmq://{host}"), h =>
                    {
                        h.Username(username);
                        h.Password(password);
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
