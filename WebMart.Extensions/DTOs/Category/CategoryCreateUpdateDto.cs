using System.ComponentModel.DataAnnotations;

namespace WebMart.Extensions.DTOs.Category
{
    public class CategoryCreateUpdateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}