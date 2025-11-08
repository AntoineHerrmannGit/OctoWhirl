namespace OctoWhirl.Core.Cache.Redis
{
    public class RedisConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public TimeSpan Expiry { get; set; }
        public string Password { get; set; }
    }
}
