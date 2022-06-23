using Microsoft.EntityFrameworkCore;
using WebMart.Microservices.CatalogService.Data;
using WebMart.Microservices.CatalogService.Models;
using WebMart.Microservices.CatalogService.Repos.Interfaces;

namespace WebMart.Microservices.CatalogService.Repos
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

        public void UpdateCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            _context.Categories.Update(category);
        }

        public ICollection<Category> GetAllCategories()
        {
            return _context.Categories.OrderBy(c => c.Name).ToList();
        }

        public ICollection<Category> GetAllCategoriesDetailed()
        {
            return _context.Categories
                .Include(c => c.SubCategories)
                .OrderBy(c => c.Name).ToList();
        }

        public Category GetCategoryById(Guid categoryId)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == categoryId);
        }

        public Category GetCategoryByIdDetailed(Guid categoryId)
        {
            return _context.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefault(c => c.Id == categoryId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}