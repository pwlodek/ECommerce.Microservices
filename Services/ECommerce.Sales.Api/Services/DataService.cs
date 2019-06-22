using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ECommerce.Sales.Api.Model;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Sales.Api.Services
{
    public class DataService : IDataService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public DataService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Customer> GetCustomerAsync(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var customersServiceHost = _configuration["CustomersServiceHost"];
            var response = await client.GetStringAsync($"http://{customersServiceHost}/api/customers/{id}");
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(response);
            return obj;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var catalogServiceHost = _configuration["CatalogServiceHost"];
            var response = await client.GetStringAsync($"http://{catalogServiceHost}/api/products");
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Product>>(response);
            return obj;
        }
    }
}
