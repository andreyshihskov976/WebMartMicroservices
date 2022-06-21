using System.ComponentModel.DataAnnotations;

namespace WebMart.Microservices.BasketService.Models
{
    public class Product
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid ExternalId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        public List<TakenProduct> TakenProducts { get; set; }
    }
}