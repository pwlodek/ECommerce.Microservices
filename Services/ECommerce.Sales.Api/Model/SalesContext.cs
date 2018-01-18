using System;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Sales.Api.Model
{
    public class SalesContext : DbContext
    {
        private readonly string _connectionString;

        public SalesContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<Order>().HasKey(p => p.OrderId);
            modelBuilder.Entity<Order>().HasMany(p => p.Items);
            modelBuilder.Entity<OrderItem>().ToTable("OrderItems");
            modelBuilder.Entity<OrderItem>().HasKey(p => p.OrderItemId);
        }

        public DbSet<Order> Orders { get; set; }
    }
}
