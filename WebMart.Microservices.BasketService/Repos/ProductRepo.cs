using WebMart.Microservices.BasketService.Data;
using WebMart.Microservices.BasketService.Models;
using WebMart.Microservices.BasketService.Repos.Interfaces;

namespace WebMart.Microservices.BasketService.Repos
{
    public class ProductRepo : IProductRepo
    {
        private readonly BasketDbContext _context;

        public ProductRepo(BasketDbContext context)
        {
            _context = context;
        }

        public void CreateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            _context.Products.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            _context.Products.Remove(product);
        }

        public void UpdateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            _context.Products.Update(product);
        }

        public ICollection<Product> GetAllProducts()
        {
            return _context.Products.OrderBy(p => p.Name).ToList();
        }

        public Product GetProductById(Guid productId)
        {
            return _context.Products.FirstOrDefault(p => p.Id == productId);
        }

        public Product GetProductByExternalId(Guid externalProductId)
        {
            return _context.Products.FirstOrDefault(p => p.ExternalId == externalProductId);
        }

        public bool ProductExists(Guid productId)
        {
            return _context.Products.Any(p => p.Id == productId);
        }

        public bool ExternalProductExists(Guid externalProductId)
        {
            return _context.Products.Any(p => p.ExternalId == externalProductId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}