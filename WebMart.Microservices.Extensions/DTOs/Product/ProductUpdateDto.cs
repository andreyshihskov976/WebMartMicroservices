using System.ComponentModel.DataAnnotations;

namespace WebMart.Microservices.Extensions.DTOs.Product
{
    public class ProductUpdateDto
    {
        [Required]
        public string Manufacturer { get; set; }

        [Required]
        public string Model { get; set; }

        public string Description { get; set; }

        public string AdditionalInfo { get; set; }

        [Required]
        public double Price { get; set; }
    }
}