using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Sales.Api.Model;

namespace ECommerce.Sales.Api.Services
{
    public interface IDataService
    {
        Task<Customer> GetCustomerAsync(int id);

        Task<IEnumerable<Product>> GetProductsAsync();
    }
}
