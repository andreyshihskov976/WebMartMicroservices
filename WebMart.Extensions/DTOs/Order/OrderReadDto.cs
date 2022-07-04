using WebMart.Extensions.DTOs.Basket;
using WebMart.Extensions.Enums;

namespace WebMart.Extensions.DTOs.Order
{
    public class OrderReadDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public BasketReadDto Basket { get; set; }
    }
}