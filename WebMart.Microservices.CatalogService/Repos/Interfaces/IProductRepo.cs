using WebMart.Microservices.CatalogService.Models;

namespace WebMart.Microservices.CatalogService.Repos.Interfaces
{
    public interface IProductRepo
    {
        bool SaveChanges();
        ICollection<Product> GetAllProducts();
        ICollection<Product> GetAllProductsDetailed();
        Product GetProductById(Guid productId);
        Product GetProductByIdDetailed(Guid productId);
        ICollection<Product> GetProductsByCategoryId(Guid categoryId);
        ICollection<Product> GetProductsByCategoryIdDetailed(Guid categoryId);
        ICollection<Product> GetProductsBySubCategoryId(Guid subCategoryId);
        ICollection<Product> GetProductsBySubCategoryIdDetailed(Guid subCategoryId);
        void CreateProduct(Guid subCategoryId, Product product);
        void DeleteProduct(Product product);
        void UpdateProduct(Product product);
        bool IsSubCategoryExists(Guid subCategoryId);
        bool IsCategoryExists(Guid categoryId);
    }
}