using WebMart.Microservices.BasketService.Data;
using WebMart.Microservices.BasketService.Models;
using WebMart.Microservices.BasketService.Repos.Interfaces;

namespace WebMart.Microservices.BasketService.Repos
{
    public class TakenProductRepo : ITakenProductRepo
    {
        private readonly BasketDbContext _context;

        public TakenProductRepo(BasketDbContext context)
        {
            _context = context;
        }

        public void CreateTakenProduct(TakenProduct takenProduct)
        {
            if (takenProduct == null)
            {
                throw new ArgumentNullException(nameof(takenProduct));
            }
            _context.TakenProducts.Add(takenProduct);
        }

        public void DeleteTakenProduct(TakenProduct takenProduct)
        {
            if (takenProduct == null)
            {
                throw new ArgumentNullException(nameof(takenProduct));
            }
            _context.TakenProducts.Remove(takenProduct);
        }

        public ICollection<TakenProduct> GetAllTakenProducts()
        {
            return _context.TakenProducts.OrderBy(tp => tp.Id).ToList();
        }

        public ICollection<TakenProduct> GetAllTakenProductsByBasketId(Guid basketId)
        {
            return _context.TakenProducts
                .Where(tp => tp.BasketId == basketId)
                .OrderBy(tp => tp.Id)
                .ToList();
        }

        public TakenProduct GetTakenProductById(Guid takenProductId)
        {
            return _context.TakenProducts.FirstOrDefault(tp => tp.Id == takenProductId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public bool TakenProductExists(Guid takenProductId)
        {
            return _context.TakenProducts.Any(tp => tp.Id == takenProductId);
        }
    }
}