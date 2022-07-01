using System.ComponentModel.DataAnnotations;

namespace WebMart.Microservices.Extensions.DTOs.SubCategory
{
    public class SubCategoryCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
    }
}