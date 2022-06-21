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

        public void AddProductToBasket(Guid basketId, Guid productId)
        {
            _context.TakenProducts.Add(new TakenProduct
            {
                BasketId = basketId,
                ProductId = productId
            });
        }

        public bool BasketExists(Guid basketId)
        {
            return _context.Baskets.Any(b => b.Id == basketId);
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

        public void DeleteProductFromBasket(Guid basketId, Guid productId)
        {
            var takenProduct = _context.TakenProducts
                .FirstOrDefault(tp => tp.BasketId == basketId && tp.ProductId == productId);
                
            _context.TakenProducts.Remove(takenProduct);
        }

        public ICollection<Basket> GetAllBaskets()
        {
            return _context.Baskets.OrderBy(b => b.Id).ToList();
        }

        public Basket GetBasketById(Guid basketId)
        {
            return _context.Baskets.FirstOrDefault(b => b.Id == basketId);
        }

        public Basket GetBasketByCustomerId(Guid customerId)
        {
            return _context.Baskets.FirstOrDefault(b => b.CustomerId == customerId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateBasket(Basket basket)
        {
            if (basket == null)
            {
                throw new ArgumentNullException(nameof(basket));
            }
            _context.Baskets.Update(basket);
        }
    }
}