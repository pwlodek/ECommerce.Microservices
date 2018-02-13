using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using ECommerce.Reporting.Api.Models;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Reporting.Api.Services
{
    public interface IOrderRepository
    {
        void Add(Order order);

        IEnumerable<Order> GetAll();
    }

    public class OrderRepository : IOrderRepository
    {
        private string _connectionString;

        public OrderRepository(IConfiguration cfg)
        {
            _connectionString = cfg["ConnectionString"];
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public void Add(Order order)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "INSERT INTO Orders (OrderId, CustomerId, FirstName, LastName, Status, Total)"
                    + " VALUES(@OrderId, @CustomerId, @FirstName, @LastName, @Status, @Total)";
                dbConnection.Open();
                dbConnection.Execute(sQuery, order);
            }
        }

        public IEnumerable<Order> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Order>("SELECT * FROM Orders");
            }
        }
    }
}
