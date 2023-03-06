#if NETSTANDARD2_0
using Newtonsoft.Json;
#else
using System.Text.Json;
#endif

namespace BunnyCDN.Net.Storage
{
    internal class Serializer
    {
        /// <summary>
        /// Deserialize the object with the most optimal serializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">JSON encoded data that will be deserialized</param>
        /// <returns>The deserialized object</returns>
        public static T Deserialize<T>(string data)
        {
#if NETSTANDARD2_0
            return JsonConvert.DeserializeObject<T>(data);
#else
            return JsonSerializer.Deserialize<T>(data);
#endif
        }

        /// <summary>
        /// Serialize the object with the most optimal serializer
        /// </summary>
        /// <param name="value">The object that will be serialized</param>
        /// <returns>JSON serialized object</returns>
        public static string Serialize<T>(object value)
        {
#if NETSTANDARD2_0
            return JsonConvert.SerializeObject(value);
#else
            return JsonSerializer.Serialize(value);
#endif
        }
    }
}
