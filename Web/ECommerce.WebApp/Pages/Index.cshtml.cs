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

        public string ServiceId { get; set; }

        public string HostName { get; set; }

        public async Task OnGetAsync()
        {
            CurrentPage = AppPage.Index;
            DisplaySearchBox = true;

            try
            {
                var response = await _productService.GetProductsAsync();
                Products = response.Payload.ToList();
                HostName = response.HostName;
                ServiceId = response.InstanceId;
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = ex.Message;
            }
        }
    }
}