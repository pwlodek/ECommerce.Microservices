using System;
using log4net.Appender;
using log4net.Core;
using MassTransit;
using ECommerce.Common;
using ECommerce.Common.Commands;

namespace ECommerce.Services.Common.Logging
{
    public class MassTransitAppender : AppenderSkeleton
    {
        public static IBus Bus { get; set; }

        public MassTransitAppender()
        {
        }

        public string ServiceName { get; set; }

        protected override void Append(LoggingEvent loggingEvent)
        {
            var message = RenderLoggingEvent(loggingEvent);

            try
            {
                SendLog(message);
            }
            catch (Exception ex)
            {
            }
        }

        private async void SendLog(string message)
        {
            var logMessage = new LogCommand() { HostName = Environment.MachineName, ServiceName = ServiceName, Message = message };
            var endpoint = await Bus.GetSendEndpoint(new Uri($"rabbitmq://{ECommerce.Common.Configuration.RabbitMqHost}/logging"));
            await endpoint.Send(logMessage);
        }
    }
}
