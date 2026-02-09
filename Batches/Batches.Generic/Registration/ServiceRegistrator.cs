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

        public static IServiceCollection RegisterBatch<TBatch>(this IServiceCollection services, string key)
            where TBatch : class, IBatch
            => services.AddKeyedScoped<IBatch, TBatch>(key);
    }
}
