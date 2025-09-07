using Microsoft.Extensions.DependencyInjection;
using OctoWhirl.Core.Cache;
using OctoWhirl.Core.Cache.CacheInMemory;
using System.Runtime.InteropServices.Marshalling;

namespace Cache.Test
{
    [TestClass]
    public sealed class CacheInMemoryTest
    {
        private IServiceProvider _provider;

        [TestInitialize]
        public void Setup()
        {
            var services = new ServiceCollection();

            // Register Services
            services.AddKeyedSingleton<ICache, CacheInMemory>(CacheType.InMemory);

            // Build Dependency-Injection
            _provider = services.BuildServiceProvider();
        }

        [TestMethod]
        public void TestCacheInMemory()
        {
            var cache = _provider.GetKeyedService<ICache>(CacheType.InMemory);
            Assert.IsNotNull(cache);

            int value = 1;
            string key = "value";

            string dictKey = "dictKey";
            int dictValue = 2;
            Dictionary<string, int> keyValues = new Dictionary<string, int>() { { dictKey, dictValue } };

            // Set single
            Assert.IsTrue(cache.Set(key, value));
            Assert.IsFalse(cache.Set(key, value));
            Assert.IsTrue(cache.Set(key, value, _override:true));

            // Set multiple
            Assert.IsTrue(cache.Set(keyValues));
            Assert.IsFalse(cache.Set(keyValues));
            Assert.IsTrue(cache.Set(keyValues, _override: true));

            // Get single
            int retrieved = cache.Get<int>(key);
            Assert.IsTrue(retrieved == value);

            // Get multiple
            Dictionary<string, int> retrievedAll = cache.Get<int>(new string[] { dictKey });
            Assert.IsTrue(retrievedAll.ContainsKey(dictKey));
            Assert.IsTrue(retrievedAll[dictKey] == dictValue);

            // Remove
            Assert.IsTrue(cache.Remove(key));
            Assert.IsTrue(cache.Remove(new string[] { dictKey }));

            // Flush
            Assert.IsTrue(cache.Flush());
        }
    }
}
