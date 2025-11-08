using Microsoft.Extensions.Configuration;
using OctoWhirl.Core.Tools.Serializer;
using OctoWhirl.Core.Tools.Technicals.Extensions;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace OctoWhirl.Core.Cache.Redis
{
    public class RedisCache : ICache
    {
        private readonly IDatabase _redis;
        private readonly TimeSpan _expiry;

        public RedisCache(RedisConfiguration config)
        {
            _expiry = config.Expiry;

            var options = new ConfigurationOptions
            {
                EndPoints = { { config.Host, config.Port } },
                Password = config.Password
            };
            _redis = ConnectionMultiplexer.Connect(options).GetDatabase();
        }

        public Task<bool> Flush()
        {
            var endpoints = _redis.Multiplexer.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = _redis.Multiplexer.GetServer(endpoint);
                server.FlushDatabase(_redis.Database);
            }
            return Task.FromResult(true);
        }

        public async Task<T> Get<T>(string key)
        {
            byte[] value = await _redis.StringGetAsync(key).ConfigureAwait(false);
            if (value.IsNullOrEmpty()) 
                return default!;
            return value.DecompressAndDeserializeFromJson<T>();
        }

        public async Task<Dictionary<string, T>> Get<T>(IEnumerable<string> keys)
        {
            var redisKeys = keys.Select(k => (RedisKey)k).ToArray();
            var values = await _redis.StringGetAsync(redisKeys).ConfigureAwait(false);
            return keys.Zip(values)
                       .ToDictionary(x => x.First, x => ((byte[])x.Second).DecompressAndDeserializeFromJson<T>());
        }

        public Task<bool> Remove(string key)
            => _redis.KeyDeleteAsync(key);

        public async Task<bool> Remove(IEnumerable<string> keys)
        {
            if (keys.IsNotEmpty())
            {
                var redisKeys = keys.Select(k => (RedisKey)k).ToArray();
                var deletedCount = await _redis.KeyDeleteAsync(redisKeys).ConfigureAwait(false);
                return deletedCount == redisKeys.LongLength;
            }
            return true;
        }

        public async Task<bool> Set<T>(string key, T value, bool _override = true)
        {
            var cacheValue = value.SerializeToJsonAndCompress();
            return await _redis.StringSetAsync(key, cacheValue, _expiry, when: _override ? When.Always : When.NotExists).ConfigureAwait(false);
        }

        public Task<bool> Set<T>(Dictionary<string, T> values, bool _override = true)
            => _redis.StringSetAsync(values.ToDictionary(x => (RedisKey)x.Key, x => (RedisValue)x.Value.SerializeToJsonAndCompress()).ToArray(), when: _override ? When.Always : When.NotExists);
    }

}
