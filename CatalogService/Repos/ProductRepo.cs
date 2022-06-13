using CatalogService.Data;
using CatalogService.Models;

namespace CatalogService.Repos
{
    public class ProductRepo : IRepository<Product>
    {
        private readonly CatalogDbContext _context;

        public ProductRepo(CatalogDbContext context)
        {
            _context = context;
        }

        public void CreateEntity(Product entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Products.Add(entity);
        }

        public void DeleteEntity(Product entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Products.Remove(entity);
        }

        public IEnumerable<Product> GetAllEntities()
        {
            return _context.Products.ToList();
        }

        public Product GetEntityById(Guid entityId)
        {
            return _context.Products.FirstOrDefault(p => p.Id == entityId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateEntity(Product entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Products.Update(entity);
        }
    }
}
