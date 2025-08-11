using Microsoft.Extensions.Configuration;
using OctoWhirl.Core.Tools.Serializer;

namespace OctoWhirl.TechnicalServices.DataService
{
    public abstract class BaseClient
    {
        protected readonly HttpClient _httpClient;

        public BaseClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _httpClient.DefaultRequestHeaders.Add(
                "User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.0.0 Safari/537.36"
            );
        }

        protected async Task<T> CallClient<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return content.DeserializeFromJson<T>() ?? throw new InvalidOperationException("Failed to deserialize response");
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
