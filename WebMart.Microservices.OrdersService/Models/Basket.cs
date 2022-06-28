using System.ComponentModel.DataAnnotations;

namespace WebMart.Microservices.OrdersService.Models
{
    public class Basket
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public Order Order { get; set; }
    }
}