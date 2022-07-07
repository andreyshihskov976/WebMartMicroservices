using WebMart.Microservices.OrdersService.Models;

namespace WebMart.Microservices.OrdersService.Repos.Interfaces
{
    public interface IOrderRepo
    {
        bool SaveChanges();
        ICollection<Order> GetAllOrders();
        Order GetOrderById(Guid orderId);
        ICollection<Order> GetOrdersByCustomerId(Guid customerId);
        void CreateOrder(Order order);
        void DeleteOrder(Order order);
        void UpdateOrder(Order order);

        Basket GetBasketById(Guid basketId);
        bool BasketExists(Guid basketId);
        void CreateMissingBasket(Basket Basket);
    }
}