using WebMart.Microservices.Extensions.DTOs.Category;
using WebMart.Microservices.Extensions.DTOs.Product;

namespace WebMart.Microservices.Extensions.DTOs.SubCategory
{
    public class SubCategoryFullyDetailedReadDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public CategoryReadDto Category { get; set; }

        public ICollection<ProductReadDto> Products { get; set; }
    }
}