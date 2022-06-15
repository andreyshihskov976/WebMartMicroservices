using CatalogService.Models;
using CatalogService.Pages;
using CatalogService.Pages.Models;

namespace CatalogService.Repos.Interfaces
{
    public interface IProductRepo
    {
        bool SaveChanges();
        ICollection<Product> GetAllProducts();
        Product GetProductById(Guid productId);
        ICollection<Product> GetProductsByCategoryId(Guid categoryId);
        ICollection<Product> GetProductsBySubCategoryId(Guid subCategoryId);
        void CreateProduct(Guid subCategoryId, Product product);
        void DeleteProduct(Product product);
        void UpdateProduct(Product product);
        bool IsSubCategoryExists(Guid subCategoryId);
        bool IsCategoryExists(Guid categoryId);
    }
}
