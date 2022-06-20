using System.ComponentModel.DataAnnotations;

namespace CatalogService.DTOs.Product
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