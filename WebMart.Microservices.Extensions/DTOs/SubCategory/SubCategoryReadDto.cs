namespace WebMart.Microservices.Extensions.DTOs.SubCategory
{
    public class SubCategoryReadDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid CategoryId { get; set; }
    }
}