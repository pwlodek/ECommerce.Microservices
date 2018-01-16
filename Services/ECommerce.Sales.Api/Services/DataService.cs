using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ECommerce.Sales.Api.Model;

namespace ECommerce.Sales.Api.Services
{
    public class DataService : IDataService
    {
        public DataService()
        {
        }

        public async Task<Customer> GetCustomerAsync(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync($"http://localhost:5001/api/customers/{id}");
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(response);
                return obj;
            }
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync($"http://localhost:5000/api/products");
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Product>>(response);
                return obj;
            }
        }
    }
}
