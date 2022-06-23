using WebMart.Microservices.Extensions.EventProcessing;

namespace WebMart.Microservices.Extensions.DTOs.Product
{
    public class ProductPublishedDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public EventType Event { get; set; }
    }
}