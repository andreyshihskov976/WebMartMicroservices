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

        public void CreateProduct(Product entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.ProductsTable.Add(entity);
        }

        public void DeleteProduct(Product entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.ProductsTable.Remove(entity);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.ProductsTable.ToList();
        }

        public Product GetProductById(Guid entityId)
        {
            return _context.ProductsTable.FirstOrDefault(p => p.Id == entityId);
        }

        public IEnumerable<Product> GetProductsByCategoryId(Guid categoryId)
        {
            return _context.ProductsTable.Where(p=>p.SubCategory.CategoryId == categoryId).ToList();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateProduct(Product entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.ProductsTable.Update(entity);
        }
    }
}
