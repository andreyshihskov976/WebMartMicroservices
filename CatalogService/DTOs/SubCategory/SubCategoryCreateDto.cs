using System.ComponentModel.DataAnnotations;

namespace CatalogService.DTOs.SubCategory
{
    public class SubCategoryCreateDto
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }
    }
}