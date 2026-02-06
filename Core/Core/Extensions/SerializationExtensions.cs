using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace Core.Technicals.Extensions
{
    public static class SerializationExtensions
    {
        #region Private Fields
        private static JsonSerializerSettings _options = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.All,
            Converters = new List<JsonConverter>()
                {
                    new StringEnumConverter()
                    {
                        AllowIntegerValues = true,
                    },
                    new IsoDateTimeConverter()
                    {
                        Culture = CultureInfo.InvariantCulture,
                        DateTimeFormat = "yyyy-MM-dd.HH:mm:ss",
                    },
                    new IsoDateTimeConverter()
                    {
                        Culture = CultureInfo.InvariantCulture,
                        DateTimeFormat = "yyyy-MM-dd",
                    }
                }
        };
        #endregion Private Fields

        #region Json Serialization
        public static string Serialize<T>(this T obj)
            => JsonConvert.SerializeObject(obj, _options);
        #endregion Json Serialization

        #region Json Deserialization
        public static T Deserialize<T>(this string obj)
            => JsonConvert.DeserializeObject<T>(obj, _options);
        #endregion Json Deserialization
    }
}
