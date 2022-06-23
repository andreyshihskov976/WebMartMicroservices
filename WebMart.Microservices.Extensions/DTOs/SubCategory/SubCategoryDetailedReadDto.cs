using WebMart.Microservices.Extensions.DTOs.Category;

namespace WebMart.Microservices.Extensions.DTOs.SubCategory
{
    public class SubCategoryDetailedReadDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public CategoryReadDto Category { get; set; }
    }
}