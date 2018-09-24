using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.WebApp.Models;
using ECommerce.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerce.WebApp.Pages
{
    public class BasketModel : PageModelBase
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;
        private static readonly Guid _serviceId = Guid.NewGuid();

        public BasketModel(IBasketService basketService, IOrderService orderService)
        {
            this._basketService = basketService;
            this._orderService = orderService;
        }

        public IList<Product> Products { get; private set; }
        
        public void OnGet()
        {
            CurrentPage = AppPage.Basket;
            Products = _basketService.GetProducts().ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _orderService.OrderBasketAsync();
            return RedirectToPage("/Index");
        }

        public string HostName => Environment.MachineName;

        public string ServiceId => _serviceId.ToString();
    }
}