using System;
using System.Data;
using System.Data.SqlClient;
using ECommerce.Catalog.Api.Models;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Catalog.Api.Services
{
    public class ProductRepository : IProductRepository
    {
        private string _connectionString;

        public ProductRepository(IConfiguration cfg)
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

        public void Add(Product prod)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "INSERT INTO Products (Name, Quantity, Price)"
                                + " VALUES(@Name, @Quantity, @Price)";
                dbConnection.Open();
                dbConnection.Execute(sQuery, prod);
            }
        }

        public IEnumerable<Product> GetAll()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Product>("SELECT * FROM Products");
            }
        }

        public IEnumerable<Product> GetAll(string filter)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<Product>($"SELECT * FROM Products P WHERE P.Name Like '%{filter}%'");
            }
        }

        public Product GetByID(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "SELECT * FROM Products"
                               + " WHERE ProductId = @Id";
                dbConnection.Open();
                return dbConnection.Query<Product>(sQuery, new { Id = id }).FirstOrDefault();
            }
        }

        public void Delete(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "DELETE FROM Products"
                             + " WHERE ProductId = @Id";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { Id = id });
            }
        }

        public void Update(Product prod)
        {
            using (IDbConnection dbConnection = Connection)
            {
                string sQuery = "UPDATE Products SET Name = @Name,"
                               + " Quantity = @Quantity, Price= @Price"
                               + " WHERE ProductId = @ProductId";
                dbConnection.Open();
                dbConnection.Query(sQuery, prod);
            }
        }
    }
}
