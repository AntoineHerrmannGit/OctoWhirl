using Microsoft.Extensions.DependencyInjection;
using OctoWhirl.Core.Cache;
using OctoWhirl.Core.Cache.Redis;
using OctoWhirl.Core.Tools.Technicals.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctoWhirl
{
    [TestClass]
    public class RedisCacheTest
    {
        private IServiceProvider _provider;

        [TestInitialize]
        public void Setup()
        {
            _provider = new ServiceCollection().AddTransient<RedisLauncher>()
                                               .AddConfiguration<RedisConfiguration>("redis_settings.json", "redis")
                                               .AddRedisCache()
                                               .AddLogging()
                                               .BuildServiceProvider();
        }

        [TestMethod]
        public async Task TestRedisLauncher()
        {
            var launcher = _provider.GetService<RedisLauncher>();
            Assert.IsTrue(launcher.StartRedisServer());
            Assert.IsTrue(launcher.StopRedisServer());
        }

        [TestMethod]
        public async Task TestRedisCache()
        {
            var launcher = _provider.GetService<RedisLauncher>();
            Assert.IsTrue(launcher.StartRedisServer());

            var cache = _provider.GetKeyedService<ICache>(CacheType.Redis);
            Assert.IsNotNull(cache);

            int value = 1;
            string key = "value";

            string dictKey = "dictKey";
            int dictValue = 2;
            Dictionary<string, int> keyValues = new Dictionary<string, int>() { { dictKey, dictValue } };

            // Set single
            Assert.IsTrue(await cache.Set(key, value).ConfigureAwait(false));
            Assert.IsFalse(await cache.Set(key, value).ConfigureAwait(false));
            Assert.IsTrue(await cache.Set(key, value, _override: true).ConfigureAwait(false));

            // Set multiple
            Assert.IsTrue(await cache.Set(keyValues).ConfigureAwait(false));
            Assert.IsFalse(await cache.Set(keyValues).ConfigureAwait(false));
            Assert.IsTrue(await cache.Set(keyValues, _override: true).ConfigureAwait(false));

            // Get single
            int retrieved = await cache.Get<int>(key).ConfigureAwait(false);
            Assert.IsTrue(retrieved == value);

            // Get multiple
            Dictionary<string, int> retrievedAll = await cache.Get<int>(new string[] { dictKey }).ConfigureAwait(false);
            Assert.IsTrue(retrievedAll.ContainsKey(dictKey));
            Assert.IsTrue(retrievedAll[dictKey] == dictValue);

            // Remove
            Assert.IsTrue(await cache.Remove(key).ConfigureAwait(false));
            Assert.IsTrue(await cache.Remove(new string[] { dictKey }).ConfigureAwait(false));

            // Flush
            Assert.IsTrue(await cache.Flush().ConfigureAwait(false));

            Assert.IsTrue(launcher.StopRedisServer());
        }
    }
}
