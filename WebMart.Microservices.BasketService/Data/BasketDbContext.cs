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
            base.OnModelCreating(modelBuilder);

            //Link for Product -> Basket
            modelBuilder
                .Entity<Product>()
                .HasMany(p => p.Baskets)
                .WithOne(p => p.Product);
            modelBuilder
                .Entity<Basket>()
                .HasOne(p => p.Product)
                .WithMany(p => p.Baskets);

            modelBuilder
                .Entity<Basket>()
                .Property(b => b.IsOrdered)
                .HasDefaultValue(false);
            modelBuilder
                .Entity<Basket>()
                .Property(b => b.Count)
                .HasDefaultValue(1);
        }
    }
}