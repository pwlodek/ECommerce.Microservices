using System;
namespace ECommerce.Sales.Api.Model
{
    public class SubmitOrder
    {
        public int CustomerId { get; set; }

        public ProductItem[] Items { get; set; }
    }

    public class ProductItem 
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
