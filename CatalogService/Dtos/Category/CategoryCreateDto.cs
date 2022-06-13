using System.ComponentModel.DataAnnotations;

namespace CatalogService.Dtos
{
    public class CategoryCreateDto
    {
        [Required]
        public string? Name { get; set; }
        
        [Required]
        public string? Description { get; set; }
    }
}
