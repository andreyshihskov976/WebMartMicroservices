using System.ComponentModel.DataAnnotations;

namespace WebMart.Microservices.CatalogService.Models
{
    public class SubCategory
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        public Category Category { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}