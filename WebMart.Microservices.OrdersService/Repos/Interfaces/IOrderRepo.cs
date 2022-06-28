using WebMart.Microservices.OrdersService.Models;

namespace WebMart.Microservices.OrdersService.Repos.Interfaces
{
    public interface IOrderRepo
    {
        bool SaveChanges();
        ICollection<Order> GetAllOrders();
        ICollection<Order> GetAllOrdersDetailed();
        Order GetOrderById(Guid orderId);
        Order GetOrderByIdDetailed(Guid orderId);
        Order GetOrderByCustomerId(int customerId);
        Order GetOrderByCustomerIdDetailed(int customerId);
        void CreateOrder(Order order);
        void DeleteOrder(Order order);
        void UpdateOrder(Order order);

        Basket GetBasketById(Guid basketId);
        bool BasketExists(Guid basketId);
        void CreateMissingBasket(Basket Basket);
    }
}