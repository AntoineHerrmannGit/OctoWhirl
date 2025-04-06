namespace OctoWhirl.Core.Tools
{
    public static class Serializer
    {
        public static T Deserialize<T>(this string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
        }
    }
}
