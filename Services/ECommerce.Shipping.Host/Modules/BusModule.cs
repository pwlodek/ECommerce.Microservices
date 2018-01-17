using System;
using Autofac;
using ECommerce.Shipping.Host.Sagas;
using MassTransit;
using MassTransit.Saga;

namespace ECommerce.Shipping.Host.Modules
{
    internal class BusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //var repository = new InMemorySagaRepository<Shipment>();

            //builder.Register();
            builder.RegisterStateMachineSagas(typeof(BusModule).Assembly);
            builder.RegisterType<ShippingSaga>();
            builder.RegisterType<InMemorySagaRepository<Shipment>>().As<ISagaRepository<Shipment>>();

            builder.Register(context =>
            {
                var rabbitHost = "localhost";
                var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri($"rabbitmq://{rabbitHost}"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    // https://stackoverflow.com/questions/39573721/disable-round-robin-pattern-and-use-fanout-on-masstransit
                    cfg.ReceiveEndpoint(host, "test" + Guid.NewGuid().ToString(), e =>
                    {
                        e.LoadFrom(context);
                        e.LoadStateMachineSagas(context);
                        //e.Saga<Shipment>(context);
                    });

                    cfg.ReceiveEndpoint(host, "shiporder", e =>
                    {
                        e.LoadFrom(context);
                        e.LoadStateMachineSagas(context);
                    });

                    cfg.ReceiveEndpoint(host, "packorder", e =>
                    {
                        e.LoadFrom(context);
                        e.LoadStateMachineSagas(context);
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
