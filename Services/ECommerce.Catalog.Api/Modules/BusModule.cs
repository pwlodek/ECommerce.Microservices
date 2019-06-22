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
                var rabbitHost = config["RabbitHost"];
                var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri($"rabbitmq://{rabbitHost}"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
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
