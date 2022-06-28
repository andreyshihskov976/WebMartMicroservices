using Microsoft.EntityFrameworkCore;
using WebMart.Microservices.BasketService.Data;
using WebMart.Microservices.BasketService.Models;
using WebMart.Microservices.BasketService.Repos.Interfaces;

namespace WebMart.Microservices.BasketService.Repos
{
    public class BasketRepo : IBasketRepo
    {
        private readonly BasketDbContext _context;

        public BasketRepo(BasketDbContext context)
        {
            _context = context;
        }

        public void CreateBasket(Basket basket)
        {
            if (basket == null)
            {
                throw new ArgumentNullException(nameof(basket));
            }
            basket.Products = new List<Product>();
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
            return _context.Baskets.OrderBy(b => b.Id).ToList();
        }

        public ICollection<Basket> GetAllBasketsDetailed()
        {
            return _context.Baskets
                .Include(b => b.Products)
                .OrderBy(b => b.Id).ToList();
        }

        public Basket GetBasketById(Guid basketId)
        {
            return _context.Baskets.FirstOrDefault(b => b.Id == basketId);
        }

        public Basket GetBasketByIdDetailed(Guid basketId)
        {
            return _context.Baskets
                .Include(b => b.Products)
                .FirstOrDefault(b => b.Id == basketId);
        }

        public Basket GetBasketByCustomerId(int customerId, bool isOrdered)
        {
            return _context.Baskets
                .FirstOrDefault(b => b.CustomerId == customerId && b.IsOrdered == isOrdered);
        }

        public Basket GetBasketByCustomerIdDetailed(int customerId, bool isOrdered)
        {
            return _context.Baskets
                .Include(b => b.Products)
                .FirstOrDefault(b => b.CustomerId == customerId && b.IsOrdered == isOrdered);
        }

        public bool BasketExists(Guid basketId)
        {
            return _context.Baskets.Any(b => b.Id == basketId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public Product GetProductById(Guid productId)
        {
            return _context.Products.FirstOrDefault(p => p.Id == productId);
        }

        public void CreateMissingProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            _context.Products.Add(product);
        }
    }
}