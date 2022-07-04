namespace WebMart.Extensions.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void Publish(object publishedDto);
    }
}
