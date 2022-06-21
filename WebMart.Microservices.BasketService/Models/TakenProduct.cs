using System.ComponentModel.DataAnnotations;

namespace WebMart.Microservices.BasketService.Models
{
    public class TakenProduct
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public Guid BasketId { get; set; }

        public Product Product { get; set; }
        
        public Basket Basket { get; set; }
    }
}