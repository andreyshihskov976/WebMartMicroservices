using System.ComponentModel.DataAnnotations;

namespace WebMart.Microservices.Extensions.DTOs.Order
{
    public class OrderCreateDto
    {
        [Required]
        public Guid BasketId { get; set; }
    }
}