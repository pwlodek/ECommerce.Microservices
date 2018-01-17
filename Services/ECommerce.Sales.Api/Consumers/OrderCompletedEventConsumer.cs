using System;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Common.Events;
using ECommerce.Sales.Api.Model;
using MassTransit;

namespace ECommerce.Sales.Api.Consumers
{
    public class OrderCompletedEventConsumer : IConsumer<OrderCompletedEvent>
    {
        public OrderCompletedEventConsumer()
        {
        }

        public async Task Consume(ConsumeContext<OrderCompletedEvent> context)
        {
            using (SalesContext ctx = new SalesContext())
            {
                var order = ctx.Orders.FirstOrDefault(t => t.OrderId == context.Message.OrderId && t.CustomerId == context.Message.CustomerId);
                if (order != null)
                {
                    order.Status = OrderStatus.Shipped;
                    ctx.SaveChanges();
                }
            }
        }
    }
}
