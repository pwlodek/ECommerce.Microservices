using System;
using Autofac;
using ECommerce.Common;
using ECommerce.Common.Commands;
using ECommerce.Common.Infrastructure.Messaging;
using ECommerce.Sales.Api.Consumers;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Sales.Api.Modules
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

                    cfg.ReceiveEndpoint(host, "sales_fanout", e =>
                    {
                        e.Consumer<OrderCompletedEventConsumer>(context);
                        e.Consumer<OrderPackedEventConsumer>(context);
                        e.Consumer<PaymentAcceptedEventConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(host, "sales_submit_orders", e =>
                    {
                        e.Consumer<SubmitOrderCommandConsumer>(context);

                        EndpointConvention.Map<SubmitOrderCommand>(e.InputAddress);
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
