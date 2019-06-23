using ECommerce.WebApp.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Services
{
    public interface IOrderService
    {
        Task OrderBasketAsync();

        Task<IList<Order>> GetOrdersAsync();

        Task<IList<OrderReportEntry>> GetOrderReportAsync();
    }

    public class OrderService : IOrderService
    {
        private readonly IBasketService _basketService;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderService(IBasketService basketService, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _basketService = basketService;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IList<Order>> GetOrdersAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var salesServiceHost = _configuration["Services:Sales"];
            var response = await client.GetStringAsync($"http://{salesServiceHost}/api/orders");
            return JsonConvert.DeserializeObject<List<Order>>(response);
        }

        public async Task<IList<OrderReportEntry>> GetOrderReportAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var reportingServiceHost = _configuration["Services:Reporting"];
            var response = await client.GetStringAsync($"http://{reportingServiceHost}/api/reporting");
            return JsonConvert.DeserializeObject<List<OrderReportEntry>>(response);
        }

        public async Task OrderBasketAsync()
        {
            var products = _basketService.GetProducts().ToList();
            if (products.Count > 0)
            {
                var productItems = new List<ProductItem>();
                var orderedProducts = from p in products
                                      group p by p.ProductId;

                foreach (var item in orderedProducts)
                {
                    productItems.Add(new ProductItem { ProductId = item.Key, Quantity = item.Count() });
                }

                var order = new SubmitOrder {
                    CustomerId = 1, // Lets assume guest
                    Items = productItems.ToArray()
                };

                var client = _httpClientFactory.CreateClient();
                var stringContent = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");
                var salesServiceHost = _configuration["Services:Sales"];
                var response = await client.PostAsync($"http://{salesServiceHost}/api/orders", stringContent);

                _basketService.Clear();
            }
        }

        // Data Structures

        public class SubmitOrder
        {
            public int CustomerId { get; set; }

            public ProductItem[] Items { get; set; }
        }

        public class ProductItem
        {
            public int ProductId { get; set; }

            public int Quantity { get; set; }
        }
    }
}
