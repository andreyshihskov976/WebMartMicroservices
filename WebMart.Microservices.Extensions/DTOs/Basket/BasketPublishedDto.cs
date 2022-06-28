using WebMart.Microservices.Extensions.Enums;

namespace WebMart.Microservices.Extensions.DTOs.Basket
{
    public class BasketPublishedDto
    {
        public Guid Id { get; set; }
        public int CustomerId { get; set; }
        public EventType Event { get; set; }
    }
}