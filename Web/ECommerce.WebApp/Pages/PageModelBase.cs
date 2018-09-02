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

        public string SearchText { get; set; }
    }
}
