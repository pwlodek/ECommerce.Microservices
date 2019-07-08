using Autofac;
using ECommerce.Common.Commands;
using ECommerce.Common.Infrastructure.Messaging;
using ECommerce.Payment.Host.Consumers;
using GreenPipes;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Payment.Host.Modules
{
    internal class AzureBusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
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

                    cfg.ReceiveEndpoint(host, "payment_fanout", e =>
                    {
                        e.Consumer<OrderSubmittedEventConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(host, "payment", e =>
                    {
                        e.Consumer<InitiatePaymentCommandConsumer>(context);
                        e.UseMessageRetry(r => r.Immediate(5));

                        EndpointConvention.Map<InitiatePaymentCommand>(e.InputAddress);
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
