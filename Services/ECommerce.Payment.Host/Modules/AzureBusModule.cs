using Autofac;
using ECommerce.Common.Commands;
using ECommerce.Payment.Host.Consumers;
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
                var config = context.Resolve<IConfiguration>();
                var serviceBusHost = config["Brokers:ServiceBus:Url"];

                var busControl = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
                {

                    var host = cfg.Host(serviceBusHost, h =>
                    {
                    });

                    cfg.ReceiveEndpoint(host, "payment_fanout", e =>
                    {
                        e.Consumer<OrderSubmittedEventConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(host, "payment_initiate_payment", e =>
                    {
                        e.Consumer<InitiatePaymentCommandConsumer>(context);

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
