using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using ECommerce.Customers.Api.Model;

namespace ECommerce.Customers.Api.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private string _connectionString;

        public CustomerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public void Add(Customer prod)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "INSERT INTO Customers (FirstName, LastName)"
                    + " VALUES(@FirstName, @LastName)";
                dbConnection.Open();
                dbConnection.Execute(sQuery, prod);
            }
        }

        public IEnumerable<Customer> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Customer>("SELECT * FROM Customers");
            }
        }

        public Customer GetByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT * FROM Customers"
                               + " WHERE CustomerId = @Id";
                dbConnection.Open();
                return dbConnection.Query<Customer>(sQuery, new { Id = id }).FirstOrDefault();
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "DELETE FROM Customers"
                             + " WHERE CustomerId = @Id";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { Id = id });
            }
        }

        public void Update(Customer prod)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "UPDATE Customers SET FirstName = @FirstName,"
                               + " LastName = @LastName"
                               + " WHERE CustomerId = @CustomerId";
                dbConnection.Open();
                dbConnection.Query(sQuery, prod);
            }
        }
    }
}
