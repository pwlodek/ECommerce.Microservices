using System;

namespace ECommerce.Common
{
    public class Configuration
    {
        public const string RabbitHostKey = "RabbitHost";

        public const string ConnectionStringKey = "ConnectionString";

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

            if (envVars.Contains(ConnectionStringKey))
            {
                ConnectionString = envVars[ConnectionStringKey].ToString();
            }
            else
            {
                ConnectionString = string.Empty;
            }
        }

        public static string RabbitMqHost { get; private set; }

        public static string ConnectionString { get; private set; }
    }
}
