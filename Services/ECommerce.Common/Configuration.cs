using System;

namespace ECommerce.Common
{
    public class Configuration
    {
        public const string RabbitHostKey = "RabbitHost";

        static Configuration()
        {
            var envVars = Environment.GetEnvironmentVariables();
            if (envVars.Contains(RabbitHostKey))
            {
                RabbitMqHost = envVars[RabbitHostKey].ToString();
            }
            else
            {
                RabbitMqHost = "localhost";
            }
        }

        public static string RabbitMqHost { get; private set; }
    }
}
