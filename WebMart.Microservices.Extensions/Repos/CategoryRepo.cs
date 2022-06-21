using WebMart.Microservices.Models;
using WebMart.Microservices.Data;
using WebMart.Microservices.Repos.Interfaces;

namespace WebMart.Microservices.Repos
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly CatalogDbContext _context;

        public CategoryRepo(CatalogDbContext context)
        {
            _context = context;
        }

        public void CreateCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            _context.Categories.Add(category);
        }

        public void DeleteCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            _context.Categories.Remove(category);
        }

        public ICollection<Category> GetAllCategories()
        {
            return _context.Categories.OrderBy(on => on.Name).ToList();
        }

        public Category GetCategoryById(Guid categoryId)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == categoryId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            _context.Categories.Update(category);
        }
    }
}