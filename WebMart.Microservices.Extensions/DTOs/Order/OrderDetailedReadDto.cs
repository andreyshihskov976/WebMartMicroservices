using WebMart.Microservices.Extensions.DTOs.Basket;
using WebMart.Microservices.Extensions.Enums;

namespace WebMart.Microservices.Extensions.DTOs.Order
{
    public class OrderDetailedReadDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public BasketReadDto Basket { get; set; }
    }
}