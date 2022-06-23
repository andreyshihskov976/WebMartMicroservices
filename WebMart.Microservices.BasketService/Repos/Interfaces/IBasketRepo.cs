using WebMart.Microservices.BasketService.Models;

namespace WebMart.Microservices.BasketService.Repos.Interfaces
{
    public interface IBasketRepo
    {
        bool SaveChanges();
        ICollection<Basket> GetAllBaskets();
        ICollection<Basket> GetAllBasketsDetailed();
        Basket GetBasketById(Guid basketId);
        Basket GetBasketByIdDetailed(Guid basketId);
        Basket GetBasketByCustomerId(int customerId, bool isOrdered);
        Basket GetBasketByCustomerIdDetailed(int customerId, bool isOrdered);
        void CreateBasket(Basket basket);
        void DeleteBasket(Basket basket);
        void UpdateBasket(Basket basket);
        bool ExternalProductExists(Guid externalProductId);
        Product GetProductByExternalId(Guid externalProductId);
    }
}