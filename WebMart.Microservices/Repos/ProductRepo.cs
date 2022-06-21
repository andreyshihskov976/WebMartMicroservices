using Microsoft.EntityFrameworkCore;
using WebMart.Microservices.Data;
using WebMart.Microservices.Models;
using WebMart.Microservices.Repos.Interfaces;

namespace WebMart.Microservices.Repos
{
    class ProductRepo : IProductRepo
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

        public ICollection<Product> GetAllProducts()
        {
            return _context.Products.OrderBy(on => on.Name).ToList();
        }

        public Product GetProductById(Guid productId)
        {
            return _context.Products.FirstOrDefault(p => p.Id == productId);
        }

        public ICollection<Product> GetProductsByCategoryId(Guid categoryId)
        {
            return _context.Products.Where(p => p.SubCategory.CategoryId == categoryId).OrderBy(on => on.Name).ToList();
        }

        public ICollection<Product> GetProductsBySubCategoryId(Guid subCategoryId)
        {
            return _context.Products.Where(p => p.SubCategoryId == subCategoryId).OrderBy(on => on.Name).ToList();
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

        public void UpdateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            _context.Products.Update(product);
        }

        public Product GetProductByIdDetailed(Guid productId)
        {
            return _context.Products
                .Include(p => p.SubCategory)
                .ThenInclude(p => p.Category)
                .FirstOrDefault(p => p.Id == productId);
        }
    }
}