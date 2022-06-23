using Microsoft.EntityFrameworkCore;
using WebMart.Microservices.CatalogService.Data;
using WebMart.Microservices.CatalogService.Models;
using WebMart.Microservices.CatalogService.Repos.Interfaces;

namespace WebMart.Microservices.CatalogService.Repos
{
    public class SubCategoryRepo : ISubCategoryRepo
    {
        private readonly CatalogDbContext _context;

        public SubCategoryRepo(CatalogDbContext context)
        {
            _context = context;
        }

        public void CreateSubCategory(Guid categoryId, SubCategory subCategory)
        {
            if (subCategory == null)
            {
                throw new ArgumentNullException(nameof(subCategory));
            }
            subCategory.CategoryId = categoryId;
            _context.SubCategories.Add(subCategory);
        }

        public void DeleteSubCategory(SubCategory subCategory)
        {
            if (subCategory == null)
            {
                throw new ArgumentNullException(nameof(subCategory));
            }
            _context.SubCategories.Remove(subCategory);
        }

        public void UpdateSubCategory(SubCategory subCategory)
        {
            if (subCategory == null)
            {
                throw new ArgumentNullException(nameof(subCategory));
            }
            _context.SubCategories.Update(subCategory);
        }

        public ICollection<SubCategory> GetAllSubCategories()
        {
            return _context.SubCategories.OrderBy(sc => sc.Name).ToList();
        }

        public ICollection<SubCategory> GetAllSubCategoriesDetailed()
        {
            return _context.SubCategories
                .Include(sc => sc.Category)
                .Include(sc => sc.Products)
                .OrderBy(sc => sc.Name).ToList();
        }

        public SubCategory GetSubCategoryById(Guid subCategoryId)
        {
            return _context.SubCategories.FirstOrDefault(sc => sc.Id == subCategoryId);
        }

        public SubCategory GetSubCategoryByIdDetailed(Guid subCategoryId)
        {
            return _context.SubCategories
                .Include(sc => sc.Category)
                .Include(sc => sc.Products)
                .FirstOrDefault(sc => sc.Id == subCategoryId);
        }

        public ICollection<SubCategory> GetSubCategoriesByCategoryId(Guid categoryId)
        {
            return _context.SubCategories
                .Where(sc => sc.CategoryId == categoryId).OrderBy(on => on.Name).ToList();
        }

        public ICollection<SubCategory> GetSubCategoriesByCategoryIdDetailed(Guid categoryId)
        {
            return _context.SubCategories
                .Include(sc => sc.Category)
                .Include(sc => sc.Products)
                .Where(sc => sc.CategoryId == categoryId).OrderBy(on => on.Name).ToList();
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