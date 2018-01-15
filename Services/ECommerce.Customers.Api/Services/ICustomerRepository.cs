using System;
using System.Collections.Generic;
using ECommerce.Customers.Api.Model;

namespace ECommerce.Customers.Api.Services
{
    public interface ICustomerRepository
    {
        void Add(Customer prod);

        IEnumerable<Customer> GetAll();

        Customer GetByID(int id);

        void Delete(int id);

        void Update(Customer prod);
    }
}
