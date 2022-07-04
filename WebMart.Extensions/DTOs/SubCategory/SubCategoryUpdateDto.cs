using System.ComponentModel.DataAnnotations;

namespace WebMart.Extensions.DTOs.SubCategory
{
    public class SubCategoryUpdateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}