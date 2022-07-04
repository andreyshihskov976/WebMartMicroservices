namespace WebMart.Extensions.SyncDataServices
{
    public interface IHttpDataService
    {
        Task<string> GetAccessToken();
        Task SendPostRequest(object request, string destination);
        Task<HttpResponseMessage> SendGetRequest(string request);
    }
}