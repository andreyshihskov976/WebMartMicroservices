using System.ComponentModel.DataAnnotations;

namespace WebMart.Extensions.DTOs.Basket
{
    public class BasketUpdateDto
    {
        [Required]
        public int Count { get; set; }
    }
}