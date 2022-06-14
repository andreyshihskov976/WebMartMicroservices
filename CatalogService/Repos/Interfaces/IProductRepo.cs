using CatalogService.Models;

namespace CatalogService.Repos.Interfaces
{
    public interface IProductRepo
    {
        bool SaveChanges();
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(Guid productId);
        IEnumerable<Product> GetProductsByCategoryId(Guid categoryId);
        IEnumerable<Product> GetProductsBySubCategoryId(Guid subCategoryId);
        void CreateProduct(Guid subCategoryId, Product product);
        void DeleteProduct(Product product);
        void UpdateProduct(Product product);
        bool IsSubCategoryExists(Guid subCategoryId);
    }
}
