using CatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Data
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> opt) : base(opt)
        {
            //Database.EnsureCreated();
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Link for Category -> SubCategory
            modelBuilder
                .Entity<Category>()
                .HasMany(p => p.SubCategories)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);
            modelBuilder
                .Entity<SubCategory>()
                .HasOne(p => p.Category)
                .WithMany(p => p.SubCategories)
                .HasForeignKey(p => p.CategoryId);

            //Link for SubCategory -> Product
            modelBuilder
                .Entity<SubCategory>()
                .HasMany(p => p.Products)
                .WithOne(p => p.SubCategory)
                .HasForeignKey(p => p.SubCategoryId);
            modelBuilder
                .Entity<Product>()
                .HasOne(p => p.SubCategory)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.SubCategoryId);
        }
    }
}
