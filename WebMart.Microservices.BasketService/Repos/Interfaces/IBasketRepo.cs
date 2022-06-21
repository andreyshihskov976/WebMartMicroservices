using WebMart.Microservices.BasketService.Models;

namespace WebMart.Microservices.BasketService.Repos.Interfaces
{
    public interface IBasketRepo
    {
        bool SaveChanges();
        ICollection<Basket> GetAllBaskets();
        Basket GetBasketById(Guid basketId);
        Basket GetBasketByCustomerId(Guid customerId);
        void CreateBasket(Basket basket);
        void DeleteBasket(Basket basket);
        bool BasketExists(Guid basketId);
    }
}