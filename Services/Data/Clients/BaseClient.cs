using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace OctoWhirl.Services.Data.Clients
{
    public abstract class BaseClient
    {
        protected readonly HttpClient _httpClient;

        public BaseClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        protected async Task<T> CallClient<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var objects = JsonConvert.DeserializeObject<T>(content);
                return objects;
            }
            catch (HttpRequestException e)
            {
                // Handle the exception as needed
                throw new Exception("Error calling the API", e);
            }
        }

        protected virtual void InitializeClient(IConfiguration configuration)
        {
            throw new NotImplementedException($"{nameof(InitializeClient)} must be implemented in child class");
        }
    }
}
