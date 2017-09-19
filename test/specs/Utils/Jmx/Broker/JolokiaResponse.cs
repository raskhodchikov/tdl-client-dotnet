using System.Net;
using RestSharp.Deserializers;

namespace TDL.Test.Specs.Utils.Jmx.Broker
{
    internal class JolokiaResponse
    {
        [DeserializeAs(Name = "value")]
        public string Value { get; set; }

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
}
