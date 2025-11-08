using Microsoft.Extensions.DependencyInjection;
using OctoWhirl.Core.Cache.CacheInMemory;
using OctoWhirl.Core.Cache.Redis;
using OctoWhirl.Core.Tools.Technicals.Extensions;

namespace OctoWhirl.Core.Cache
{
    public static class ServiceRegistrator
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services)
            => services.AddConfiguration<RedisConfiguration>("redis_settings.json", "redis")
                       .AddKeyedSingleton<ICache, RedisCache>(CacheType.Redis)
                       .AddSingleton<RedisLauncher>();

        public static IServiceCollection AddInMemoryCache(this IServiceCollection services)
            => services.AddKeyedScoped<ICache, InMemoryCache>(CacheType.InMemory);
    }
}
