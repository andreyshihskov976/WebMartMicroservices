namespace WebMart.Microservices.Extensions.DTOs.Product
{
    public class ProductReadDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public Guid SubCategoryId { get; set; }
    }
}