using System.ComponentModel.DataAnnotations;

namespace WebMart.Microservices.DTOs.SubCategory
{
    public class SubCategoryCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}