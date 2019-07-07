using System;
using System.Collections.Generic;
using System.Linq;
using CorrelationId;
using ECommerce.Common.Commands;
using ECommerce.Common.Extensions;
using ECommerce.Sales.Api.Model;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Sales.Api.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IBus _bus;
        private readonly SalesContext _salesContext;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public OrdersController(IBus bus, SalesContext salesContext, ICorrelationContextAccessor correlationContextAccessor)
        {
            _bus = bus;
            _salesContext = salesContext;
            _correlationContextAccessor = correlationContextAccessor;
        }

        // GET api/orders
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            var orders = _salesContext.Orders.Include(o => o.Items).ToList();
            return orders;
        }

        // POST api/orders
        [HttpPost]
        public async void Post([FromBody]SubmitOrder submittedOrder)
        {
            var command = new SubmitOrderCommand()
            {
                CorrelationId = _correlationContextAccessor.CorrelationContext.CorrelationId.ToGuid(),
                CustomerId = submittedOrder.CustomerId,
                Items = submittedOrder.Items.Select(t => new Item() { ProductId = t.ProductId, Quantity = t.Quantity }).ToArray()
            };

            await _bus.Send(command);
        }
    }
}
