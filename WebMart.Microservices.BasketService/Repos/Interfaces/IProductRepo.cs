using WebMart.Microservices.BasketService.Models;

namespace WebMart.Microservices.BasketService.Repos.Interfaces
{
    public interface IProductRepo
    {
        bool SaveChanges();
        ICollection<Product> GetAllProducts();
        Product GetProductById(Guid productId);
        Product GetProductByExternalId(Guid externalProductId);
        void CreateProduct(Product product);
        void DeleteProduct(Product product);
        void UpdateProduct(Product product);
        bool ExternalProductExists(Guid externalProductId);
    }
}