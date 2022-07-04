using System.ComponentModel.DataAnnotations;
using WebMart.Extensions.Enums;

namespace WebMart.Microservices.OrdersService.Models
{
    public class Order
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid BasketId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        public Basket Basket { get; set; }
    }
}