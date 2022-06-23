using System.ComponentModel.DataAnnotations;

namespace WebMart.Microservices.BasketService.Models
{
    public class Basket
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public bool IsOrdered {get; set;}

        public List<Product> Products { get; set; }
    }
}