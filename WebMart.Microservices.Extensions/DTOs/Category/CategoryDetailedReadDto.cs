using WebMart.Microservices.Extensions.DTOs.SubCategory;

namespace WebMart.Microservices.Extensions.DTOs.Category
{
    public class CategoryDetailedReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<SubCategoryReadDto> SubCategories { get; set; }
    }
}