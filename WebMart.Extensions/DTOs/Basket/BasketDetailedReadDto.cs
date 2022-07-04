namespace WebMart.Extensions.DTOs.Basket
{
    public class BasketDetailedReadDto
    {
        public Guid Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductCount { get; set; }
        public bool IsOrdered { get; set; }
        public List<BasketProductReadDto> Products { get; set; }
    }
}