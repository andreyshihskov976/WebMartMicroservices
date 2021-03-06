using System.ComponentModel.DataAnnotations;

namespace WebMart.Extensions.DTOs.Basket
{
    public class BasketCreateDto
    {
        [Required]
        public Guid CustomerId { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        public int Count { get; set; }
    }
}