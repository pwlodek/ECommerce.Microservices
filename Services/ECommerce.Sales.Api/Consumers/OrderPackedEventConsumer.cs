using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Common.Events;
using ECommerce.Sales.Api.Model;
using log4net;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Sales.Api.Consumers
{
    public class OrderPackedEventConsumer : IConsumer<OrderPackedEvent>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(OrderPackedEventConsumer));

        private IConfiguration _cfg;

        public OrderPackedEventConsumer(IConfiguration cfg)
        {
            _cfg = cfg;
        }

        public async Task Consume(ConsumeContext<OrderPackedEvent> context)
        {
            using (SalesContext ctx = new SalesContext(_cfg["ConnectionString"]))
            {
                var order = ctx.Orders.FirstOrDefault(t => t.OrderId == context.Message.OrderId && t.CustomerId == context.Message.CustomerId);
                if (order != null)
                {
                    order.Status |= OrderStatus.Packed;
                    ctx.SaveChanges();
                }
            }

            Logger.Info($"Order {context.Message.OrderId} for customer {context.Message.CustomerId} has been marked as packed");
        }
    }
}
