using WebMart.Microservices.Extensions.DTOs.Category;
using WebMart.Microservices.Extensions.DTOs.SubCategory;

namespace WebMart.Microservices.Extensions.DTOs.Product
{
    public class ProductDetailedReadDto
    {
        public Guid Id { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public string Description { get; set; }

        public string AdditionalInfo { get; set; }

        public double Price { get; set; }

        public CategoryReadDto Category { get; set; }

        public SubCategoryReadDto SubCategory { get; set; }
    }
}