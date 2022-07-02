namespace WebMart.Microservices.Extensions.DTOs.Basket
{
    public class BasketReadDto
    {
        public Guid Id { get; set; }
        public int CustomerId { get; set; }
        public int ProductCount { get; set; }
        public double TotalCost { get; set; }
    }
}