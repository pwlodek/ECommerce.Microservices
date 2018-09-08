using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebApp.Pages
{
    public abstract class PageModelBase : PageModel
    {
        public bool DisplaySearchBox { get; set; }

        [BindProperty]
        public string SearchText { get; set; }

        public bool HasError { get; protected set; }

        public string ErrorMessage { get; protected set; }

        public AppPage CurrentPage { get; protected set; }

        public string GetActive(AppPage page)
        {
            return (CurrentPage == page ? "active" : "inactive");
        }
    }

    public enum AppPage
    {
        Index,
        Orders,
        Basket
    }
}
