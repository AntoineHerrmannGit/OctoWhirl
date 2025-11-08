namespace OctoWhirl.Core.Cache
{
    public interface ICache
    {
        Task<T> Get<T>(string key);
        Task<Dictionary<string, T>> Get<T>(IEnumerable<string> keys);

        Task<bool> Set<T>(string key, T value, bool _override = false);
        Task<bool> Set<T>(Dictionary<string, T> values, bool _override = false);
        Task<bool> Remove(string key);
        Task<bool> Remove(IEnumerable<string> keys);
        Task<bool> Flush();
    }
}
