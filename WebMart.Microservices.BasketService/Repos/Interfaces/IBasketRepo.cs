using WebMart.Microservices.BasketService.Models;

namespace WebMart.Microservices.BasketService.Repos.Interfaces
{
    public interface IBasketRepo
    {
        bool SaveChanges();
        ICollection<Basket> GetAllBaskets();
        Basket GetBasketById(Guid basketId);
        ICollection<Basket> GetAllBasketsByCustomerId(Guid customerId);
        Basket GetOpenBasketByCustomerId(Guid customerId);
        ICollection<Product> GetProductsInBasket(Guid basketId);
        void CreateBasket(Basket basket);
        void DeleteBasket(Basket basket);
        void UpdateBasket(Basket basket);
        bool BasketExists(Guid basketId);
        bool OpenBasketForCustomerExists(Guid customerId);

        Product GetProductById(Guid productId);
        bool ProductExists(Guid productId);
        void CreateMissingProduct(Product product);
    }
}