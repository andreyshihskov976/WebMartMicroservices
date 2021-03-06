using WebMart.Extensions.Enums;

namespace WebMart.Extensions.DTOs.Basket
{
    public class BasketPublishedDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public int Count { get; set; }
        public double TotalCost { get; set; }
        public EventType Event { get; set; }
    }
}