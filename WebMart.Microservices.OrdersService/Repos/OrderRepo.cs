using Microsoft.EntityFrameworkCore;
using WebMart.Microservices.OrdersService.Data;
using WebMart.Microservices.OrdersService.Models;
using WebMart.Microservices.OrdersService.Repos.Interfaces;

namespace WebMart.Microservices.OrdersService.Repos
{
    public class OrderRepo : IOrderRepo
    {
        private readonly OrdersDbContext _context;

        public OrderRepo(OrdersDbContext context)
        {
            _context = context;
        }

        public void CreateOrder(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            _context.Orders.Add(order);
        }

        public void DeleteOrder(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            _context.Orders.Remove(order);
        }

        public void UpdateOrder(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            _context.Orders.Update(order);
        }

        public ICollection<Order> GetAllOrders()
        {
            return _context.Orders.OrderBy(o => o.OrderDate).ToList();
        }

        public Order GetOrderById(Guid orderId)
        {
            return _context.Orders.FirstOrDefault(o => o.Id == orderId);
        }

        public ICollection<Order> GetOrdersByCustomerId(int customerId)
        {
            return _context.Orders.Where(o => o.Basket.CustomerId == customerId).OrderBy(o => o.OrderDate).ToList();
        }

        public Basket GetBasketById(Guid basketId)
        {
            return _context.Baskets.FirstOrDefault(b => b.Id == basketId);
        }

        public bool BasketExists(Guid basketId)
        {
            return _context.Baskets.Any(b => b.Id == basketId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void CreateMissingBasket(Basket basket)
        {
            if (basket == null)
            {
                throw new ArgumentNullException(nameof(basket));
            }
            _context.Baskets.Add(basket);
        }
    }
}