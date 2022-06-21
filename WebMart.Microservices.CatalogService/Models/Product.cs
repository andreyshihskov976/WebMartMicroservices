using System.ComponentModel.DataAnnotations;

namespace WebMart.Microservices.CatalogService.Models
{
    public class Product
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public Guid? SubCategoryId { get; set; }

        public SubCategory SubCategory { get; set; }
    }
}