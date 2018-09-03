using ECommerce.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Services
{
    public interface IBasketService
    {
        void AddProduct(Product product);

        IEnumerable<Product> GetProducts();
    }

    public class BasketService : IBasketService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BasketService(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }

        public void AddProduct(Product product)
        {
            var serializedBasket = _httpContextAccessor.HttpContext.Session.GetString("Basket");
            var basket = serializedBasket != null ? 
                JsonConvert.DeserializeObject<List<Product>>(serializedBasket) : 
                new List<Product>();

            basket.Add(product);

            _httpContextAccessor.HttpContext.Session.SetString("Basket", JsonConvert.SerializeObject(basket));
        }

        public IEnumerable<Product> GetProducts()
        {
            var serializedBasket = _httpContextAccessor.HttpContext.Session.GetString("Basket");
            var basket = serializedBasket != null ?
                JsonConvert.DeserializeObject<List<Product>>(serializedBasket) :
                new List<Product>();

            return basket;
        }
    }
}
