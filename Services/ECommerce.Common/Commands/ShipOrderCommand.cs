using System;
namespace ECommerce.Common.Commands
{
    public class ShipOrderCommand
    {
        public ShipOrderCommand()
        {
        }

        public int OrderId { get; set; }

        public int CustomerId { get; set; }
    }
}
