using CatalogService.Data;
using CatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Repos
{
    public class CategoryRepo : IRepository<Category>
    {
        private readonly CatalogDbContext _context;

        public CategoryRepo(CatalogDbContext context)
        {
            _context = context;
        }

        public void CreateEntity(Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Categories.Add(entity);
        }

        public void DeleteEntity(Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Categories.Remove(entity);
        }

        public IEnumerable<Category> GetAllEntities()
        {
            return _context.Categories.ToList();
        }

        public Category GetEntityById(Guid entityId)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == entityId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateEntity(Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Categories.Update(entity);
        }
    }
}
