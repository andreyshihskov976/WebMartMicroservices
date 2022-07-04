using System.ComponentModel.DataAnnotations;

namespace WebMart.Extensions.DTOs.Order
{
    public class OrderCreateDto
    {
        [Required]
        public Guid BasketId { get; set; }
    }
}