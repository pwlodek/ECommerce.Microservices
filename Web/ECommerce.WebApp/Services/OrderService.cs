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
    }

    public class OrderService : IOrderService
    {
        private readonly IBasketService _basketService;
        private readonly IConfiguration _configuration;

        public OrderService(IBasketService basketService, IConfiguration configuration)
        {
            this._basketService = basketService;
            this._configuration = configuration;
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

                using (var client = new HttpClient())
                {
                    var stringContent = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");
                    var salesServiceHost = _configuration["SalesServiceHost"];
                    var response = await client.PostAsync($"http://{salesServiceHost}/api/orders", stringContent);

                    _basketService.Clear();
                }
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
