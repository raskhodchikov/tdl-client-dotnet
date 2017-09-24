using Newtonsoft.Json;
using TDL.Client.Abstractions.Response;

namespace TDL.Client.Serialization
{
    public class ResponseJson
    {
        [JsonProperty("result")]
        public object Result { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        public static ResponseJson From(IResponse response) =>
            new ResponseJson
            {
                Result = response.Result,
                Error = null,
                Id = response.Id
            };
    }
}
