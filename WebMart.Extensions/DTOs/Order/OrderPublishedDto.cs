using WebMart.Extensions.Enums;

namespace WebMart.Extensions.DTOs.Order
{
    public class OrderPublishedDto
    {
        public Guid Id { get; set; }
        public Guid BasketId { get; set; }
        public EventType Event { get; set; }
    }
}