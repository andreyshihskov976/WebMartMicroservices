using System.ComponentModel.DataAnnotations;

namespace WebMart.Microservices.OrdersService.Models
{
    public class Basket
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        public int Count { get; set; }

        public double TotalCost { get; set; }

        public Order Order { get; set; }
    }
}