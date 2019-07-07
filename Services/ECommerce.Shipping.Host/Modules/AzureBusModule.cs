using System;
using Autofac;
using ECommerce.Common;
using ECommerce.Common.Commands;
using ECommerce.Common.Infrastructure.Messaging;
using ECommerce.Shipping.Host.Configuration;
using ECommerce.Shipping.Host.Consumers;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ECommerce.Shipping.Host.Modules
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

                    cfg.ReceiveEndpoint(host, "shipping_fanout", e =>
                    {
                        e.LoadStateMachineSagas(context);
                    });

                    cfg.ReceiveEndpoint(host, "shipping_order", e =>
                    {
                        e.Consumer<ShipOrderCommandConsumer>(context);
                        e.Consumer<InitiateOrderPackingCommandConsumer>(context);

                        EndpointConvention.Map<ShipOrderCommand>(e.InputAddress);
                        EndpointConvention.Map<InitiateOrderPackingCommand>(e.InputAddress);
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
