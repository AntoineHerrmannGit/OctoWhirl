using OctoWhirl.Core.Tools.Serializer;
using System.Collections.Concurrent;

namespace OctoWhirl.Core.Cache.CacheInMemory
{
    public class InMemoryCache : ICache
    {
        private Dictionary<string, byte[]> _container;
        
        public InMemoryCache()
        {
            _container = new Dictionary<string, byte[]>();
        }

        #region Get
        public Task<T> Get<T>(string key)
        {
            if (_container.ContainsKey(key))
                return Task.FromResult(_container[key].DecompressAndDeserializeFromJson<T>());
            return Task.FromResult(default(T));
        }
        public async Task<Dictionary<string, T>> Get<T>(IEnumerable<string> keys)
        {
            var result = new ConcurrentDictionary<string, T>();
            foreach(var key in keys)
                result[key] = await Get<T>(key).ConfigureAwait(false);
            return result.ToDictionary();
        }
        #endregion Get

        #region Set
        public Task<bool> Set<T>(string key, T value, bool _override = false)
        {
            if(!_override && _container.ContainsKey(key))
                return Task.FromResult(false);
            _container[key] = value.SerializeToJsonAndCompress();
            return Task.FromResult(true);
        }

        public async Task<bool> Set<T>(Dictionary<string, T> values, bool _override = false)
        {
            var buffer = new Dictionary<string, byte[]>();
            var results = new List<bool>();
            foreach(var kv in values)
            {
                if (_container.ContainsKey(kv.Key))
                    buffer[kv.Key] = _container[kv.Key];
                results.Add(await Set(kv.Key, kv.Value, _override).ConfigureAwait(false));
            }

            if(results.Any(r => !r))
            {
                foreach(var kv in buffer)
                    _container[kv.Key] = kv.Value;
                return false;
            }
            return true;
        }
        #endregion Set

        #region Remove
        public Task<bool> Remove(string key) 
            => Task.FromResult(_container.ContainsKey(key) && _container.Remove(key));

        public Task<bool> Remove(IEnumerable<string> keys) 
            => Task.FromResult(keys.All(key => _container.ContainsKey(key) &&  _container.Remove(key)));
        #endregion Remove

        #region Flush
        public Task<bool> Flush()
        {
            _container = new Dictionary<string, byte[]>();
            return Task.FromResult(true);
        }
        #endregion Flush
    }
}
