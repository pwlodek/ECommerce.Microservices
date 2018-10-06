using System;
using log4net.Appender;
using log4net.Core;
using MassTransit;
using ECommerce.Common;
using ECommerce.Common.Commands;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.Services.Common.Logging
{
    public class MassTransitAppender : AppenderSkeleton
    {
        public static IBus Bus { get; set; }

        public string ServiceName { get; set; }

        private List<LoggingEvent> _buffer = new List<LoggingEvent>();

        protected override void Append(LoggingEvent loggingEvent)
        {
            if (Bus == null)
            {
                _buffer.Add(loggingEvent);
            }
            else
            {
                if (_buffer.Count > 0)
                {
                    var buffer = _buffer.ToArray();
                    _buffer.Clear();

                    // send buffer
                    foreach (var item in buffer)
                    {
                        Process(item);
                    }
                }
                Process(loggingEvent);
            }
        }

        private async void Process(LoggingEvent loggingEvent)
        {
            var message = RenderLoggingEvent(loggingEvent);

            try
            {
                await SendLog(message);
            }
            catch (Exception ex)
            {
            }
        }

        private async Task SendLog(string message)
        {
            var logMessage = new LogCommand() { HostName = Environment.MachineName, ServiceName = ServiceName, Message = message };
            var endpoint = await Bus.GetSendEndpoint(new Uri($"rabbitmq://{ECommerce.Common.Configuration.RabbitMqHost}/logging"));
            await endpoint.Send(logMessage);
        }
    }
}
