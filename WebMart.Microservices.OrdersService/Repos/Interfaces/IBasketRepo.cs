using WebMart.Microservices.OrdersService.Models;

namespace WebMart.Microservices.OrdersService.Repos.Interfaces
{
    public interface IBasketRepo
    {
        bool SaveChanges();
        ICollection<Basket> GetAllBaskets();
        Basket GetBasketById(Guid basketId);
        void CreateBasket(Basket basket);
        void UpdateBasket(Basket basket);
        void DeleteBasket(Basket basket);
        bool BasketExists(Guid basketId);
    }
}