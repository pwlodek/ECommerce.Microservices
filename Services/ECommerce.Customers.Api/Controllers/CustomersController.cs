using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorrelationId;
using ECommerce.Customers.Api.Model;
using ECommerce.Customers.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Customers.Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        readonly ICustomerRepository _customerRepository;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public CustomersController(ICustomerRepository customerRepository, ICorrelationContextAccessor correlationContextAccessor)
        {
            _customerRepository = customerRepository;
            _correlationContextAccessor = correlationContextAccessor;
        }

        // GET api/customers
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return _customerRepository.GetAll();
        }

        // GET api/customers/5
        [HttpGet("{id}")]
        public Customer Get(int id)
        {
            var correlationId = _correlationContextAccessor.CorrelationContext.CorrelationId;
            return _customerRepository.GetByID(id);
        }

        // POST api/customers
        [HttpPost]
        public void Post([FromBody]Customer value)
        {
            _customerRepository.Add(value);
        }

        // PUT api/customers/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Customer value)
        {
            _customerRepository.Update(value);
        }

        // DELETE api/customers/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _customerRepository.Delete(id);
        }
    }
}
