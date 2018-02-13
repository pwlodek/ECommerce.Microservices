using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Reporting.Api.Models;
using ECommerce.Reporting.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Reporting.Api.Controllers
{
    [Route("api/[controller]")]
    public class ReportingController : Controller
    {
        readonly IOrderRepository _orderRepository;

        public ReportingController(IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
        }

        // GET api/reporting
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            return _orderRepository.GetAll();
        }
    }
}
