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
    public class IndexModel : PageModelBase
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            this._productService = productService;
        }

        public IList<Product> Products { get; private set; }

        public async Task OnGetAsync()
        {
            DisplaySearchBox = true;

            try
            {
                var products = await _productService.GetProductsAsync();
                Products = products.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}