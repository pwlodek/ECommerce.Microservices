using System;
using System.Threading;
using System.Data.SqlClient;

using RabbitMQ.Client;

namespace ECommerce.Services.Common.Configuration
{
    public class DependencyAwaiter
    {
        public DependencyAwaiter()
        {
        }

        public void WaitForSql(string connectionString)
        {
            var waiter = new SqlAwaiter();
            waiter.WaitForSql(connectionString);
        }

        public void WaitForRabbit(string host)
        {
            var waiter = new RabbitAwaiter();
            waiter.WaitForRabbit(host);
        }
    }

    internal class SqlAwaiter
    {
        private int _count = 1;

        public void WaitForSql(string connectionString)
        {
            for (int i = 0; i < 50; i++)
            {
                if (i > 0)
                {
                    Thread.Sleep(1000 * _count);
                    _count *= 2; //exponental backoff

                    Console.WriteLine("Trying to connect to SQL database: " + i);
                }
                try
                {
                    var conn = new SqlConnection(connectionString);
                    conn.Open();
                    conn.Close();
                    return;
                }
                catch (Exception ex)
                {
                }
            }

            throw new Exception("Could not connect to Rabbit MQ.");
        }
    }

    internal class RabbitAwaiter
    {
        private int _count = 1;

        public void WaitForRabbit(string host)
        {
            var factory = new ConnectionFactory() { HostName = host, Port = 5672, UserName = "guest", Password = "guest" };
            GetConnection(factory);
        }

        private void GetConnection(ConnectionFactory factory)
        {
            for (int i = 0; i < 50; i++)
            {
                if (i > 0)
                {
                    Thread.Sleep(1000 * _count);
                    _count *= 2; //exponental backoff

                    Console.WriteLine("Trying to connect to rabbit mq: " + i);
                }
                try
                {
                    var conn = factory.CreateConnection();
                    conn.Close();
                    return;
                }
                catch (Exception ex)
                {
                }
            }

            throw new Exception("Could not connect to Rabbit MQ.");
        }
    }
}
