using Newtonsoft.Json;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace OctoWhirl.Core.Tools.Serializer
{
    public static class SerializationExtension
    {
        private static readonly Encoding _encode = Encoding.UTF8;

        #region Serialization
        public static string SerializeToJson(this object obj) => JsonConvert.SerializeObject(obj);
        public static byte[] SerializeToJsonAndCompress(this object obj)
        {
            string jsonString = obj.SerializeToJson();
            byte[] jsonBytes = _encode.GetBytes(jsonString);

            using (var outputStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                    gzipStream.Write(jsonBytes, 0, jsonBytes.Length);
                return outputStream.ToArray();
            }
        }
        #endregion Serialization

        #region Deserialization
        public static TResponse DeserializeFromJson<TResponse>(this string json) => JsonConvert.DeserializeObject<TResponse>(json);
        public static T DecompressAndDeserializeFromJson<T>(this byte[] compressedData)
        {
            using (var inputStream = new MemoryStream(compressedData))
            using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            using (var reader = new StreamReader(gzipStream, _encode))
                return reader.ReadToEnd().DeserializeFromJson<T>();
        }

        #endregion Deserialization
    }
}
