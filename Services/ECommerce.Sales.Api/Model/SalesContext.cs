using System;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Sales.Api.Model
{
    public class SalesContext : DbContext
    {
        public SalesContext(DbContextOptions<SalesContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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
