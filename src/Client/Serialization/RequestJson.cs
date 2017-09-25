using Newtonsoft.Json;
using TDL.Client.Abstractions;

namespace TDL.Client.Serialization
{
    public class RequestJson
    {
        [JsonProperty("method")]
        public string MethodName { get; set; }

        [JsonProperty("params")]
        public string[] Params { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        public Request To() =>
            new Request
            {
                MethodName = MethodName,
                Params = Params,
                Id = Id
            };

        public static RequestJson Deserialize(string value)
        {
            try
            {
                return JsonConvert.DeserializeObject<RequestJson>(value);
            }
            catch (JsonReaderException ex)
            {
                throw new DeserializationException("Invalid message format", ex);
            }
        }
    }
}
