using Newtonsoft.Json;

namespace OctoWhirl.Core.Tools
{
    public static class Serializer
    {
        public static string Serialize<T>(this T obj) where T: class
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public static T Deserialize<T>(this string json) where T : class
        {
            T result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }
    }
}
