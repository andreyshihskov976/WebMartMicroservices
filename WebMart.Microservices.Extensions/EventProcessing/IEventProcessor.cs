namespace WebMart.Microservices.Extensions.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}
