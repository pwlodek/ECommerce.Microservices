using System;
using System.Net.Http;
using System.Threading.Tasks;
using ECommerce.Reporting.Api.Models;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Reporting.Api.Services
{
    public interface IDataService
    {
        Task<Customer> GetCustomerAsync(int id);
    }

    public class DataService : IDataService
    {
        private readonly IConfiguration _configuration;

        public DataService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task<Customer> GetCustomerAsync(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var customersServiceHost = _configuration["CustomersServiceHost"];
                var response = await client.GetStringAsync($"http://{customersServiceHost}/api/customers/{id}");
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(response);
                return obj;
            }
        }
    }
}
