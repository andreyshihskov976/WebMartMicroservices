using WebMart.Microservices.Extensions.EventProcessing;

namespace WebMart.Microservices.Extensions.DTOs.Events
{
    public class GenericEventDto
    {
        public EventType Event { get; set; }
    }
}