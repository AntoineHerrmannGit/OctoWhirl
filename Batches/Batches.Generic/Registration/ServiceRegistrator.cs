using Batches.Generic.Factory;
using Batches.Generic.Interfaces;
using Batches.Generic.Runner;
using Batches.Generic.Tracking;
using Microsoft.Extensions.DependencyInjection;

namespace Batches.Generic.Registration
{
    public static class ServiceRegistrator
    {
        public static IServiceCollection RegisterBatchRunner(this IServiceCollection services)
            => services.AddScoped<IBatchRunner, BatchRunner>()
                       .AddScoped<IBatchFactory, BatchFactory>()
                       .AddScoped<Tracker>();

        // It is extremely important to register the batches as transcients
        // to not reuse the same instance if a batch runs another batch of same nature
        // deeper in the tree
        public static IServiceCollection RegisterBatch<TBatch>(this IServiceCollection services, string key)
            where TBatch : class, IBatch
            => services.AddKeyedTransient<IBatch, TBatch>(key);
    }
}
