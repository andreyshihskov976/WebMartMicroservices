namespace WebMart.Microservices.Extensions.DTOs.Basket
{
    public class BasketReadDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public int ProductsCount { get; set; }
        public double SummaryPrice { get; set; }
    }
}