using WebMart.Microservices.Models;

namespace WebMart.Microservices.Repos.Interfaces
{
    public interface ICategoryRepo
    {
        bool SaveChanges();
        ICollection<Category> GetAllCategories();
        Category GetCategoryById(Guid categoryId);
        void CreateCategory(Category category);
        void DeleteCategory(Category category);
        void UpdateCategory(Category category);
    }
}