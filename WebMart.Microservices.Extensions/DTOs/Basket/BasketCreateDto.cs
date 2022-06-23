using System.ComponentModel.DataAnnotations;

namespace WebMart.Microservices.Extensions.DTOs.Basket
{
    public class BasketCreateDto
    {
        [Required]
        public int CustomerId { get; set; }
    }
}