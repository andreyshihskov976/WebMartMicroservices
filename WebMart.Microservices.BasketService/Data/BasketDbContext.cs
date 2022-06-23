using Microsoft.EntityFrameworkCore;
using WebMart.Microservices.BasketService.Models;

namespace WebMart.Microservices.BasketService.Data
{
    public class BasketDbContext : DbContext
    {
        public BasketDbContext(DbContextOptions<BasketDbContext> opt) : base(opt) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Basket> Baskets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Link for Product -> TakenProduct
            modelBuilder
                .Entity<Product>()
                .HasMany(p => p.Baskets)
                .WithMany(p => p.Products);
            modelBuilder
                .Entity<Basket>()
                .HasMany(p => p.Products)
                .WithMany(p => p.Baskets);

            modelBuilder
                .Entity<Basket>()
                .Property(b => b.IsOrdered)
                .HasDefaultValue(false);
        }
    }
}