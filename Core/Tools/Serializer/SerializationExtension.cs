using Newtonsoft.Json;
#pragma warning disable CS8603 // Possible null reference return.

namespace OctoWhirl.Core.Tools.Serializer
{
    public static class SerializationExtension
    {
        public static string SerializeToJson(this object obj) => JsonConvert.SerializeObject(obj);
        public static TResponse DeserializeFromJson<TResponse>(this string json) => JsonConvert.DeserializeObject<TResponse>(json);
    }
}
