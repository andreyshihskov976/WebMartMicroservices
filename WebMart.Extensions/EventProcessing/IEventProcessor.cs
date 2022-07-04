namespace WebMart.Extensions.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}
