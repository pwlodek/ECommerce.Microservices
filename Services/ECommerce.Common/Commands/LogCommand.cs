using System;
namespace ECommerce.Common.Commands
{
    public class LogCommand
    {
        public LogCommand()
        {
        }

        public string ServiceName { get; set; }

        public string HostName { get; set; }

        public string Message { get; set; }
    }
}
