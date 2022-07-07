using System.ComponentModel.DataAnnotations;
using WebMart.Extensions.Enums;

namespace WebMart.Extensions.DTOs.Order
{
    public class OrderUpdateDto
    {
        [Required]
        public OrderStatus Status { get; set; }
    }
}