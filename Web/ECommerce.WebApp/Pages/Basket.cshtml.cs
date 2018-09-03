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

        public BasketModel(IBasketService basketService)
        {
            this._basketService = basketService;
        }

        public IList<Product> Products { get; private set; }
        
        public void OnGet()
        {
            Products = _basketService.GetProducts().ToList();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/Index");
        }
    }
}