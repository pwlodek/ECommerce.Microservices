using System;
using System.Threading.Tasks;
using ECommerce.Common.Commands;
using MassTransit;

namespace ECommerce.Logging.Host.Consumers
{
    public class LogCommandConsumer : IConsumer<LogCommand>
    {
        public LogCommandConsumer()
        {
        }

        public async Task Consume(ConsumeContext<LogCommand> context)
        {
            // Print out message so that Docker's logging system can capture it
            Console.Write(context.Message.Message);

            // Here we can forward logs to a centralized logging system, Azure, etc.
        }
    }
}
