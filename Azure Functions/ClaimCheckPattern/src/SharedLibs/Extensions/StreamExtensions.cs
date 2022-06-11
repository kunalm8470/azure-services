using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SharedLibs.Extensions
{
    public static class StreamExtensions
    {
        public static async IAsyncEnumerable<T> StreamPerJsonObject<T>(this Stream s)
        {
            using StreamReader sr = new(s);

            using JsonTextReader reader = new(sr);
            
            while (await reader.ReadAsync())
            {
                if (reader.TokenType == JsonToken.StartObject)
                {
                    // Load each object from the stream and do something with it
                    yield return JObject.Load(reader).ToObject<T>();
                }
            }
        }
    }
}
