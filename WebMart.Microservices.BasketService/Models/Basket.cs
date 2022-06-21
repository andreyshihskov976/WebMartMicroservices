using System.ComponentModel.DataAnnotations;

namespace WebMart.Microservices.BasketService.Models
{
    public class Basket
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Customer { get; set; }

        public List<TakenProduct> TakenProducts { get; set; }
    }
}