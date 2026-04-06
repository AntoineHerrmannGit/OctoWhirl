using Batches.Generic.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Batches.Generic.Factory
{
    internal class BatchFactory : IBatchFactory
    {
        private readonly ILogger<BatchFactory> _logger;
        private readonly IServiceProvider _provider;

        public BatchFactory(ILogger<BatchFactory> logger, IServiceProvider provider)
        {
            _logger = logger;
            _provider = provider;
        }

        public IBatch Create(string name)
        {
            _logger.LogInformation($"Creating new batch \"{name}\".");
            return _provider.GetRequiredKeyedService<IBatch>(name);
        }
    }
}
