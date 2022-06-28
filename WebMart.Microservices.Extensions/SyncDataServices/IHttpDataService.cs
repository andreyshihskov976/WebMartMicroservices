namespace WebMart.Microservices.Extensions.SyncDataServices
{
    public interface IHttpDataService
    {
        Task SendPostRequest(object request, string destination);
        Task<HttpResponseMessage> SendGetRequest(string request);
    }
}