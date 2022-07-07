namespace WebMart.Extensions.DTOs.Order
{
    public class OrderBasketReadDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public int Count { get; set; }
        public double TotalCost { get; set; }
    }
}