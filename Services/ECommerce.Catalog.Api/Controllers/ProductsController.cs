using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Catalog.Api.Models;
using ECommerce.Catalog.Api.Services;
using ECommerce.Services.Common.Identity;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Catalog.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private static ILog Logger = LogManager.GetLogger(typeof(ProductsController));

        private readonly IProductRepository _productRepository;
        private readonly IIdentityService _identityService;

        public ProductsController(IProductRepository productRepository, IIdentityService identityService)
        {
            this._identityService = identityService;
            this._productRepository = productRepository;
        }

        // GET api/products
        [HttpGet]
        public IActionResult Get(string filter = null, bool includeServiceInfo = false)
        {
            Logger.Debug($"Instance {_identityService.InstanceId} is returning products.");

            var products = !string.IsNullOrWhiteSpace(filter) ?
                _productRepository.GetAll(filter) : _productRepository.GetAll();
            
            if (includeServiceInfo)
            {
                return Ok(new ServiceResponse<IEnumerable<Product>> {
                    HostName = _identityService.HostName,
                    InstanceId = _identityService.InstanceId,
                    Payload = products
                });
            }
            else
            {
                return Ok(products);
            }            
        }

        // GET api/products/5
        [HttpGet("{id}")]
        public Product Get(int id)
        {
            return _productRepository.GetByID(id);
        }

        // POST api/products
        [HttpPost]
        public void Post([FromBody]Product value)
        {
            _productRepository.Add(value);
        }

        // PUT api/products/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Product value)
        {
            _productRepository.Update(value);
        }

        // DELETE api/products/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _productRepository.Delete(id);
        }
    }
}
