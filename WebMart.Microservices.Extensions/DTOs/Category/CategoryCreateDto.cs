using System.ComponentModel.DataAnnotations;

namespace WebMart.Microservices.Extensions.DTOs.Category
{
    public class CategoryCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
    }
}