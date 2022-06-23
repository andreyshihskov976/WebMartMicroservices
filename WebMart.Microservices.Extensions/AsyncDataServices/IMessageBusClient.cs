namespace WebMart.Microservices.Extensions.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void Publish(object publishedDto);
    }
}
