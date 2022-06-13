using CatalogService.Data;
using CatalogService.Models;

namespace CatalogService.Repos
{
    public class SubCategoryRepo : IRepository<SubCategory>
    {
        private readonly CatalogDbContext _context;

        public SubCategoryRepo(CatalogDbContext context)
        {
            _context = context;
        }

        public void CreateEntity(SubCategory entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.SubCategories.Add(entity);
        }

        public void DeleteEntity(SubCategory entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.SubCategories.Remove(entity);
        }

        public IEnumerable<SubCategory> GetAllEntities()
        {
            return _context.SubCategories.ToList();
        }

        public SubCategory GetEntityById(Guid entityId)
        {
            return _context.SubCategories.FirstOrDefault(sc => sc.Id == entityId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateEntity(SubCategory entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.SubCategories.Update(entity);
        }
    }
}
