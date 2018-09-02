using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IHttpContextAccessor _accessor;

        public BasketController(IHttpContextAccessor accessor)
        {
            this._accessor = accessor;
        }

        // POST: api/Basket
        [HttpPost]
        public void Post(string id)
        {
            
        }
    }
}