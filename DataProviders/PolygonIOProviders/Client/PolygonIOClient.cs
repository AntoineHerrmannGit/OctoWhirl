using Core.Http.HttpClients;
using Core.Http.HttpRequests;
using Core.Technicals.Tools;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PolygonIOProviders.Configuration;
using PolygonIOProviders.Interfaces;

namespace PolygonIOProviders.Client
{
    internal class PolygonIOClient : BaseHttpClient, IPolygonIOClient
    {
        private readonly ILogger<PolygonIOClient> _logger;
        private readonly string _apiKey;

        private const int _retryDelay = 60000;
        private const int _retryAttempts = 3;

        #region Ctor
        public PolygonIOClient(ILogger<PolygonIOClient> logger, IOptions<PolygonIOConfiguration> configuration)
            : base()
        {
            _logger = logger;
            _apiKey = configuration.Value.ApiKey;
        }
        #endregion Ctor

        #region Public Methods
        public Task<TPolygonIOModel> CallApi<TPolygonIOModel>(HttpRequest request, CancellationToken cancellationToken = default)
            where TPolygonIOModel : class
        {
            _logger.LogInformation($"Calling PolygonIO Api...");
            request.AddParameter("apiKey", _apiKey);

            // PolygonIO has a threshold of 5 requests per minute. 
            // Thus, we retry until the api is no more throttled
            return TaskTools.Retry(() => Execute<TPolygonIOModel>(request, cancellationToken), attempts: _retryAttempts, delay: _retryDelay);
        }
        #endregion Public Methods
    }
}
