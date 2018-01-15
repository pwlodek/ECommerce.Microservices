using System;
using System.Threading;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace ECommerce.Customers.Api.Configuration
{
    public static class MassTransitConfiguration
    {
        public static void AddMassTransitUsingRabbit(this IServiceCollection services)
        {
            WaitForRabbit();

            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                sbc.ExchangeType = ExchangeType.Fanout;
            });

            services.Add(new ServiceDescriptor(typeof(IBus), bus));
            services.Add(new ServiceDescriptor(typeof(IBusControl), bus));
            services.Add(new ServiceDescriptor(typeof(IPublishEndpoint), bus));
        }

        public static void UseMassTransit(this IApplicationBuilder app)
        {
            var bus = app.ApplicationServices.GetService<IBusControl>();
            var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();

            bus.Start();
            lifetime.ApplicationStopping.Register(() => bus.Stop());
        }

        private static void WaitForRabbit()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "guest", Password = "guest" };
            GetConnection(factory);
        }

        private static void GetConnection(ConnectionFactory factory)
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
