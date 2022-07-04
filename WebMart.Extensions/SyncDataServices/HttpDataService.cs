using System.Text;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace WebMart.Extensions.SyncDataServices
{
    public class HttpDataService : IHttpDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpDataService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GetAccessToken()
        {
            // discover endpoints from metadata
            var disco = await _httpClient.GetDiscoveryDocumentAsync
            (
                _configuration.GetSection("IdentityParameters")["IdentityServerHost"]
            );
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
            }

            // request token
            var tokenResponse = await _httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = _configuration.GetSection("IdentityParameters")["ClientId"],
                ClientSecret = _configuration.GetSection("IdentityParameters")["ClientSecret"],
                Scope = _configuration.GetSection("IdentityParameters")["AllowedScope"]
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
            }

            return tokenResponse.AccessToken;
        }

        public async Task<HttpResponseMessage> SendGetRequest(string request)
        {
            _httpClient.SetBearerToken(await GetAccessToken());
            return await _httpClient.GetAsync(request);
        }

        public async Task SendPostRequest(object message, string destination)
        {
            var httpContent = new StringContent
            (
                JsonConvert.SerializeObject(message),
                Encoding.UTF8,
                "application/json"
            );
            await _httpClient.PostAsync($"{destination}", httpContent);
        }
    }
}