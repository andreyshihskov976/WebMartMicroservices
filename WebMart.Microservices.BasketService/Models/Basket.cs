using System.ComponentModel.DataAnnotations;

namespace WebMart.Microservices.BasketService.Models
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

        [Required]
        public bool IsOrdered { get; set; }

        public Product Product { get; set; }
    }
}