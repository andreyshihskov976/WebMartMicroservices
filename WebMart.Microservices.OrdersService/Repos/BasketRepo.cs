using WebMart.Microservices.OrdersService.Data;
using WebMart.Microservices.OrdersService.Models;
using WebMart.Microservices.OrdersService.Repos.Interfaces;

namespace WebMart.Microservices.OrdersService.Repos
{
    public class BasketRepo : IBasketRepo
    {
        private readonly OrdersDbContext _context;

        public BasketRepo(OrdersDbContext context)
        {
            _context = context;
        }

        public void CreateBasket(Basket basket)
        {
            if (basket == null)
            {
                throw new ArgumentNullException(nameof(basket));
            }
            _context.Baskets.Add(basket);
        }

        public void DeleteBasket(Basket basket)
        {
            if (basket == null)
            {
                throw new ArgumentNullException(nameof(basket));
            }
            _context.Baskets.Remove(basket);
        }

        public void UpdateBasket(Basket basket)
        {
            if (basket == null)
            {
                throw new ArgumentNullException(nameof(basket));
            }
            _context.Baskets.Update(basket);
        }

        public ICollection<Basket> GetAllBaskets()
        {
            return _context.Baskets.OrderBy(b => b.CustomerId).ToList();
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
    }
}