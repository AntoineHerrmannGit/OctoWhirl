using OctoWhirl.Core.Tools.Serializer;

namespace OctoWhirl.Core.Cache.CacheInMemory
{
    public class CacheInMemory : ICache
    {
        private Dictionary<string, byte[]> _container;
        
        public CacheInMemory()
        {
            _container = new Dictionary<string, byte[]>();
        }

        #region Get
        public T Get<T>(string key)
        {
            if (_container.ContainsKey(key))
                return _container[key].DecompressAndDeserializeFromJson<T>();
            return default(T);
        }
        public Dictionary<string, T> Get<T>(IEnumerable<string> keys)
            => keys.AsParallel().ToDictionary(
                key => key,
                key => Get<T>(key)
            );
        #endregion Get

        #region Set
        public bool Set<T>(string key, T value, bool _override = false)
        {
            if(!_override && _container.ContainsKey(key))
                return false;
            _container[key] = value.SerializeToJsonAndCompress();
            return true;
        }

        public bool Set<T>(Dictionary<string, T> values, bool _override = false)
        {
            var buffer = new Dictionary<string, byte[]>();
            var results = new List<bool>();
            foreach(var kv in values)
            {
                if (_container.ContainsKey(kv.Key))
                    buffer[kv.Key] = _container[kv.Key];
                results.Add(Set(kv.Key, kv.Value, _override));
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
        public bool Remove(string key) 
            => _container.ContainsKey(key) && _container.Remove(key);

        public bool Remove(IEnumerable<string> keys) 
            => keys.All(key => _container.ContainsKey(key) &&  _container.Remove(key));
        #endregion Remove

        #region Flush
        public bool Flush()
        {
            _container = new Dictionary<string, byte[]>();
            return true;
        }
        #endregion Flush
    }
}
