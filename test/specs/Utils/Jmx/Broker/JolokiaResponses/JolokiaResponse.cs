using System.Net;
using RestSharp.Deserializers;

namespace TDL.Test.Specs.Utils.Jmx.Broker.JolokiaResponses
{
    internal class JolokiaResponse
    {
        [DeserializeAs(Name = "status")]
        public HttpStatusCode Status { get; set; }

        [DeserializeAs(Name = "timestamp")]
        public long Timestamp { get; set; }

        [DeserializeAs(Name = "error_type")]
        public string ErrorType { get; set; }

        [DeserializeAs(Name = "error")]
        public string Error { get; set; }

        [DeserializeAs(Name = "stacktrace")]
        public string Stacktrace { get; set; }
    }

    internal class JolokiaResponse<T> : JolokiaResponse
    {
        [DeserializeAs(Name = "value")]
        public T Value { get; set; }
    }
}
