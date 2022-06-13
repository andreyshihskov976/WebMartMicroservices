using System.ComponentModel.DataAnnotations;

namespace CatalogService.Models
{
    public class Category
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }

        public List<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
    }
}
