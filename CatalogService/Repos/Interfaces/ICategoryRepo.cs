using CatalogService.Models;

namespace CatalogService.Repos.Interfaces
{
    public interface ICategoryRepo
    {
        bool SaveChanges();
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(Guid categoryId);
        void CreateCategory(Category category);
        void DeleteCategory(Category category);
        void UpdateCategory(Category category);
    }
}
