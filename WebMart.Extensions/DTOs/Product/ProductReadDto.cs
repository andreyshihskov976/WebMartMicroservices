namespace WebMart.Extensions.DTOs.Product
{
    public class ProductReadDto
    {
        public Guid Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }
    }
}