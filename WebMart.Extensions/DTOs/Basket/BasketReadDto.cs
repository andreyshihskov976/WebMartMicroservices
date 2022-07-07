using WebMart.Extensions.DTOs.Product;

namespace WebMart.Extensions.DTOs.Basket
{
    public class BasketReadDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public ProductReadDto Product { get; set; }
        public int Count { get; set; }
        public double TotalCost { get; set; }
    }
}