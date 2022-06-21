using System.ComponentModel.DataAnnotations;

namespace WebMart.Microservices.CatalogService.Models
{
    public class Category
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }
    }
}