using WebMart.Microservices.BasketService.Models;

namespace WebMart.Microservices.BasketService.Repos.Interfaces
{
    public interface IBasketRepo
    {
        bool SaveChanges();
        ICollection<Basket> GetAllBaskets();
        Basket GetBasketById(Guid basketId);
        ICollection<Basket> GetBasketsByCustomerId(Guid customerId);
        void CreateBasket(Basket basket);
        void DeleteBasket(Basket basket);
        void UpdateBasket(Basket basket);
        bool BasketExists(Guid basketId);

        Product GetProductById(Guid productId);
        bool ProductExists(Guid productId);
        void CreateMissingProduct(Product product);
    }
}