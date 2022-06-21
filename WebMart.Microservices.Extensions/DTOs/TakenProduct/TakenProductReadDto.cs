namespace WebMart.Microservices.Extensions.DTOs.TakenProduct
{
    public class TakenProductReadDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid BasketId { get; set; }
    }
}