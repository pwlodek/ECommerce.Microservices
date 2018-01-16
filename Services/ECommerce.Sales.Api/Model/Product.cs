using System;
namespace ECommerce.Sales.Api.Model
{
    public class Product
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }
    }
}
