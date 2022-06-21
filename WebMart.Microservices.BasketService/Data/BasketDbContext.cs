using Microsoft.EntityFrameworkCore;
using WebMart.Microservices.BasketService.Models;

namespace WebMart.Microservices.BasketService.Data
{
    public class BasketDbContext : DbContext
    {
        public BasketDbContext(DbContextOptions<BasketDbContext> opt) : base(opt) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<TakenProduct> TakenProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Link for Product -> TakenProduct
            modelBuilder
                .Entity<Product>()
                .HasMany(p => p.TakenProducts)
                .WithOne(p => p.Product)
                .HasForeignKey(p => p.ProductId);
            modelBuilder
                .Entity<TakenProduct>()
                .HasOne(p => p.Product)
                .WithMany(p => p.TakenProducts)
                .HasForeignKey(p => p.ProductId);

            //Link for TakenProduct -> Basket
            modelBuilder
                .Entity<Basket>()
                .HasMany(p => p.TakenProducts)
                .WithOne(p => p.Basket)
                .HasForeignKey(p => p.BasketId);
            modelBuilder
                .Entity<TakenProduct>()
                .HasOne(p => p.Basket)
                .WithMany(p => p.TakenProducts)
                .HasForeignKey(p => p.BasketId);
        }
    }
}