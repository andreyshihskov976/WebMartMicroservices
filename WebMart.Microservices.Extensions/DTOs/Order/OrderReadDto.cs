using WebMart.Microservices.Extensions.Enums;

namespace WebMart.Microservices.Extensions.DTOs.Order
{
    public class OrderReadDto
    {
        public Guid Id { get; set; }
        public Guid BasketId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }        
    }
}