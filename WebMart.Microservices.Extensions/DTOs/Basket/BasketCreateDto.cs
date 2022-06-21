using System.ComponentModel.DataAnnotations;

namespace WebMart.Microservices.Extensions.DTOs.Basket
{
    public class BasketCreateDto
    {
        [Required]
        public Guid CustomerId { get; set; }
    }
}