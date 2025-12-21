using Core.Http.HttpRequests;
using Core.Technicals.Extensions;

namespace Core.Http.HttpClients
{
    public abstract class BaseHttpClient
    {
        private readonly HttpClient _httpClient;

        #region Ctor
        public BaseHttpClient(string? url = null, TimeSpan? timeout = default)
        {
            _httpClient = new HttpClient()
            {
                Timeout = timeout ?? TimeSpan.FromMinutes(10),
            };

            if (!url.IsNullOrEmpty())
                _httpClient.BaseAddress = new Uri(url);
        }
        #endregion Ctor

        #region Public HttpClient Methods
        public void SetUrl(string url)
            => _httpClient.BaseAddress = new Uri(url ?? throw new ArgumentNullException(nameof(url)));

        public void SetTimeout(TimeSpan timeout)
            => _httpClient.Timeout = timeout;
        #endregion Public HttpClient Methods

        #region Public Methods
        public async Task<T> Execute<T>(HttpRequest request, CancellationToken cancellationToken = default)
        {
            if (request is null) 
                throw new ArgumentNullException(nameof(request));

            var httpMessage = request.Build();
            cancellationToken.ThrowIfCancellationRequested();

            var response = await _httpClient.SendAsync(httpMessage, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return content.Deserialize<T>();
        }
        #endregion Public Methods
    }
}
