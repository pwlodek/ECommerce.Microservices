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
        private readonly IHttpClientFactory _httpClientFactory;

        public DataService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            this._configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Customer> GetCustomerAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("DefaultClient");
            var customersServiceHost = _configuration["Services:Customer"];
            var response = await client.GetStringAsync($"http://{customersServiceHost}/api/customers/{id}");
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(response);
            return obj;
        }
    }
}
