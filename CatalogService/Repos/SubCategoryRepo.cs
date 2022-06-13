using CatalogService.Data;
using CatalogService.Models;
using CatalogService.Repos.Interfaces;

namespace CatalogService.Repos
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

        public IEnumerable<SubCategory> GetAllSubCategories()
        {
            return _context.SubCategories.ToList();
        }

        public SubCategory GetSubCategoryById(Guid subCategoryId)
        {
            return _context.SubCategories.FirstOrDefault(sc => sc.Id == subCategoryId);
        }

        public bool IsCategoryExists(Guid categoryId)
        {
            return _context.Categories.Any(c => c.Id == categoryId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateSubCategory(SubCategory subCategory)
        {
            if (subCategory == null)
            {
                throw new ArgumentNullException(nameof(subCategory));
            }
            _context.SubCategories.Update(subCategory);
        }
    }
}
