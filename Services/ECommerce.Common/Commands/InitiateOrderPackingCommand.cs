using System;
namespace ECommerce.Common.Commands
{
    public class InitiateOrderPackingCommand
    {
        public InitiateOrderPackingCommand()
        {
        }

        public int OrderId { get; set; }

        public int CustomerId { get; set; }
    }
}
