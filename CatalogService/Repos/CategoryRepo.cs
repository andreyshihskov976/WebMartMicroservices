using CatalogService.Data;
using CatalogService.Models;
using CatalogService.Repos.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Repos
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
            _context.CategoriesTable.Add(category);
        }

        public void DeleteCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            _context.CategoriesTable.Remove(category);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.CategoriesTable.ToList();
        }

        public Category GetCategoryById(Guid categoryId)
        {
            return _context.CategoriesTable.FirstOrDefault(c => c.Id == categoryId);
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
            _context.CategoriesTable.Update(category);
        }
    }
}
