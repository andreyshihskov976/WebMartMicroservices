using WebMart.Extensions.Enums;

namespace WebMart.Extensions.DTOs.Product
{
    public class ProductPublishedDto
    {
        public Guid Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }
        public EventType Event { get; set; }
    }
}