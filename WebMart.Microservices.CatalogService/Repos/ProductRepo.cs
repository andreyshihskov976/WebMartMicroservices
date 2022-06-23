using Microsoft.EntityFrameworkCore;
using WebMart.Microservices.CatalogService.Data;
using WebMart.Microservices.CatalogService.Models;
using WebMart.Microservices.CatalogService.Repos.Interfaces;

namespace WebMart.Microservices.CatalogService.Repos
{
    public class ProductRepo : IProductRepo
    {
        private readonly CatalogDbContext _context;

        public ProductRepo(CatalogDbContext context)
        {
            _context = context;
        }

        public void CreateProduct(Guid subCategoryId, Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            product.SubCategoryId = subCategoryId;
            _context.Products.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            _context.Products.Remove(product);
        }

        public void UpdateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            _context.Products.Update(product);
        }

        public ICollection<Product> GetAllProducts()
        {
            return _context.Products.OrderBy(on => on.Name).ToList();
        }

        public ICollection<Product> GetAllProductsDetailed()
        {
            return _context.Products
                .Include(p => p.SubCategory)
                .ThenInclude(sc => sc.Category)
                .OrderBy(p => p.Name).ToList();
        }

        public Product GetProductById(Guid productId)
        {
            return _context.Products.FirstOrDefault(p => p.Id == productId);
        }

        public Product GetProductByIdDetailed(Guid productId)
        {
            return _context.Products
                .Include(p => p.SubCategory)
                .ThenInclude(sc => sc.Category)
                .FirstOrDefault(p => p.Id == productId);
        }

        public ICollection<Product> GetProductsByCategoryId(Guid categoryId)
        {
            return _context.Products
                .Where(p => p.SubCategory.CategoryId == categoryId)
                .OrderBy(p => p.Name).ToList();
        }

        public ICollection<Product> GetProductsByCategoryIdDetailed(Guid categoryId)
        {
            return _context.Products
                .Include(p => p.SubCategory)
                .ThenInclude(sc => sc.Category)
                .Where(p => p.SubCategory.CategoryId == categoryId)
                .OrderBy(p => p.Name).ToList();
        }

        public ICollection<Product> GetProductsBySubCategoryId(Guid subCategoryId)
        {
            return _context.Products
                .Where(p => p.SubCategoryId == subCategoryId)
                .OrderBy(p => p.Name).ToList();
        }

        public ICollection<Product> GetProductsBySubCategoryIdDetailed(Guid subCategoryId)
        {
            return _context.Products
                .Include(p => p.SubCategory)
                .ThenInclude(sc => sc.Category)
                .Where(p => p.SubCategoryId == subCategoryId)
                .OrderBy(p => p.Name).ToList();
        }

        public bool IsSubCategoryExists(Guid subCategoryId)
        {
            return _context.SubCategories.Any(sc => sc.Id == subCategoryId);
        }

        public bool IsCategoryExists(Guid categoryId)
        {
            return _context.Categories.Any(c => c.Id == categoryId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}