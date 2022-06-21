using WebMart.Microservices.CatalogService.Models;

namespace WebMart.Microservices.CatalogService.Repos.Interfaces
{
    public interface ISubCategoryRepo
    {
        bool SaveChanges();
        ICollection<SubCategory> GetAllSubCategories();
        SubCategory GetSubCategoryById(Guid subCategoryId);
        ICollection<SubCategory> GetSubCategoriesByCategoryId(Guid categoryId);
        void CreateSubCategory(Guid categoryId, SubCategory subCategory);
        void DeleteSubCategory(SubCategory subCategory);
        void UpdateSubCategory(SubCategory subCategory);
        bool IsCategoryExists(Guid categoryId);
    }
}