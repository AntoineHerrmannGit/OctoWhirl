using OctoWhirl.Client.Requests;
using OctoWhirl.Core.Tools.Serializer;

namespace OctoWhirl.Client
{
    public abstract class BaseClient : IDisposable
    {
        private readonly HttpClient _httpClient;

        private readonly string _url;

        protected BaseClient(string url)
        {
            _httpClient = new HttpClient();
            _url = url;
        }

        protected IHttpRequest CreateRequest(string route, HttpRequestMethod method)
        {
            return method switch
            {
                HttpRequestMethod.GET => new HttpGETRequest(_url, route),
                HttpRequestMethod.POST => new HttpPOSTRequest(_url, route),
                _ => throw new NotSupportedException(method.ToString())
            };
        }

        protected async Task<TResponse> Execute<TResponse>(IHttpRequest request)
        {
            var httpRequest = request.Build();
            var httpResponse = await _httpClient.SendAsync(httpRequest).ConfigureAwait(false);

            httpResponse.EnsureSuccessStatusCode();

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            return jsonResponse.DeserializeFromJson<TResponse>() ?? throw new InvalidOperationException("Failed to deserialize response");
        }

        #region IDisposable
        public void Dispose()
        {
            _httpClient?.Dispose();
        }
        #endregion IDisposable
    }
}
