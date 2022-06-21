using System.ComponentModel.DataAnnotations;

namespace WebMart.Microservices.Extensions.DTOs.Product
{
    public class ProductCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public double Price { get; set; }
    }
}