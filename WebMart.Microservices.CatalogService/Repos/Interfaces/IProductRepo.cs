using WebMart.Microservices.CatalogService.Models;

namespace WebMart.Microservices.CatalogService.Repos.Interfaces
{
    public interface IProductRepo
    {
        bool SaveChanges();
        ICollection<Product> GetAllProducts();
        Product GetProductById(Guid productId);
        Product GetProductByIdDetailed(Guid productId);
        ICollection<Product> GetProductsByCategoryId(Guid categoryId);
        ICollection<Product> GetProductsBySubCategoryId(Guid subCategoryId);
        void CreateProduct(Guid subCategoryId, Product product);
        void DeleteProduct(Product product);
        void UpdateProduct(Product product);
        bool IsSubCategoryExists(Guid subCategoryId);
        bool IsCategoryExists(Guid categoryId);
    }
}