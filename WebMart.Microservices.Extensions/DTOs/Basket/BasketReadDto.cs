namespace WebMart.Microservices.Extensions.DTOs.Basket
{
    public class BasketReadDto
    {
        public Guid Id { get; set; }
        public int CustomerId { get; set; }
        public bool IsOrdered { get; set; }
    }
}