using CatalogService.Models;

namespace CatalogService.Repos.Interfaces
{
    public interface ISubCategoryRepo
    {
        bool SaveChanges();
        IEnumerable<SubCategory> GetAllSubCategories();
        SubCategory GetSubCategoryById(Guid subCategoryId);
        IEnumerable<SubCategory> GetSubCategoriesByCategoryId(Guid categoryId);
        void CreateSubCategory(Guid categoryId, SubCategory subCategory);
        void DeleteSubCategory(SubCategory subCategory);
        void UpdateSubCategory(SubCategory subCategory);
        bool IsCategoryExists(Guid categoryId);
    }
}
