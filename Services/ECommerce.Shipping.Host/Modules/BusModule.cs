using System;
using System.Threading;
using Autofac;
using ECommerce.Common;
using ECommerce.Shipping.Host.Consumers;
using MassTransit;
using RabbitMQ.Client;

namespace ECommerce.Shipping.Host.Modules
{
    internal class BusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            WaitForRabbit(Configuration.RabbitMqHost);

            builder.Register(context =>
            {
                var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri($"rabbitmq://{Configuration.RabbitMqHost}"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    // https://stackoverflow.com/questions/39573721/disable-round-robin-pattern-and-use-fanout-on-masstransit
                    cfg.ReceiveEndpoint(host, "ecommerce_main_fanout" + Guid.NewGuid().ToString(), e =>
                    {
                        e.LoadStateMachineSagas(context);
                    });

                    cfg.ReceiveEndpoint(host, "shiporder", e =>
                    {
                        e.Consumer<ShipOrderCommandConsumer>(context);
                    });

                    cfg.ReceiveEndpoint(host, "packorder", e =>
                    {
                        e.Consumer<InitiateOrderPackingCommandConsumer>(context);
                    });
                });

                return busControl;
            })
            .SingleInstance()
            .As<IPublishEndpoint>()
            .As<IBusControl>()
            .As<IBus>();
        }

        private void WaitForRabbit(string host)
        {
            var factory = new ConnectionFactory() { HostName = host, Port = 5672, UserName = "guest", Password = "guest" };
            GetConnection(factory);
        }

        private void GetConnection(ConnectionFactory factory)
        {
            for (int i = 0; i < 50; i++)
            {
                if (i > 0)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Trying to connect to rabbit mq: " + i);
                }
                try
                {
                    var conn = factory.CreateConnection();
                    conn.Close();
                    return;
                }
                catch (Exception ex)
                {
                }
            }

            throw new Exception("Could not connect.");
        }
    }
}
