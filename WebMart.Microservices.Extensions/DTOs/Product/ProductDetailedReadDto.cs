using WebMart.Microservices.Extensions.DTOs.SubCategory;

namespace WebMart.Microservices.Extensions.DTOs.Product
{
    public class ProductDetailedReadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public SubCategoryDetailedReadDto SubCategory { get; set; }
    }
}