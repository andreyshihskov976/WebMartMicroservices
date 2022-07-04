using Microsoft.EntityFrameworkCore;
using WebMart.Microservices.OrdersService.Models;
using WebMart.Extensions.Enums;

namespace WebMart.Microservices.OrdersService.Data
{
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> opt) : base(opt){ }

        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Link for Basket -> Order
            modelBuilder
                .Entity<Basket>()
                .HasOne(b => b.Order)
                .WithOne(o => o.Basket)
                .HasForeignKey<Order>(o => o.BasketId);
            modelBuilder
                .Entity<Order>()
                .HasOne(o => o.Basket)
                .WithOne(b => b.Order)
                .HasForeignKey<Order>(o => o.BasketId);

            modelBuilder
                .Entity<Order>()
                .Property(o => o.Status)
                .HasDefaultValue(OrderStatus.Accepted);
        }
    }
}