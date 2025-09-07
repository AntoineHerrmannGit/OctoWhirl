namespace OctoWhirl.Core.Cache
{
    public interface ICache
    {
        T Get<T>(string key);
        Dictionary<string, T> Get<T>(IEnumerable<string> keys);

        bool Set<T>(string key, T value, bool _override = false);
        bool Set<T>(Dictionary<string, T> values, bool _override = false);
        bool Remove(string key);
        bool Remove(IEnumerable<string> keys);
        bool Flush();
    }
}
