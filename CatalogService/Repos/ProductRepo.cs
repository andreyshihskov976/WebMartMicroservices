using CatalogService.Data;
using CatalogService.Models;
using CatalogService.Repos.Interfaces;

namespace CatalogService.Repos
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

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product GetProductById(Guid productId)
        {
            return _context.Products.FirstOrDefault(p => p.Id == productId);
        }

        public IEnumerable<Product> GetProductsByCategoryId(Guid categoryId)
        {
            return _context.Products.Where(p=>p.SubCategory.CategoryId == categoryId).ToList();
        }

        public IEnumerable<Product> GetProductsBySubCategoryId(Guid subCategoryId)
        {
            return _context.Products.Where(p=>p.SubCategoryId == subCategoryId).ToList();
        }

        public bool IsSubCategoryExists(Guid subCategoryId)
        {
            return _context.SubCategories.Any(sc => sc.Id == subCategoryId);
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
    }
}
