using WebMart.Microservices.CatalogService.Models;

namespace WebMart.Microservices.CatalogService.Repos.Interfaces
{
    public interface ICategoryRepo
    {
        bool SaveChanges();
        ICollection<Category> GetAllCategories();
        ICollection<Category> GetAllCategoriesDetailed();
        Category GetCategoryById(Guid categoryId);
        Category GetCategoryByIdDetailed(Guid categoryId);
        void CreateCategory(Category category);
        void DeleteCategory(Category category);
        void UpdateCategory(Category category);
    }
}