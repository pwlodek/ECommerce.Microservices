using Autofac;
using ECommerce.Common.Infrastructure.Messaging;
using ECommerce.Reporting.Api.Consumers;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Reporting.Api.Modules
{
    internal class AzureBusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MessageCorrelationContextAccessor>().SingleInstance().As<IMessageCorrelationContextAccessor>();
            builder.Register(context =>
            {
                var correlationContextAccessor = context.Resolve<IMessageCorrelationContextAccessor>();
                var config = context.Resolve<IConfiguration>();
                var serviceBusHost = config["Brokers:ServiceBus:Url"];

                var busControl = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
                {
                    var host = cfg.Host(serviceBusHost, h =>
                    {
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
