using RestSharp.Deserializers;

namespace TDL.Test.Specs.Utils.Jmx.Broker.JolokiaResponses
{
    internal class JolokiaBrowseResponseValueItem
    {
        [DeserializeAs(Name = "Text")]
        public string Text { get; set; }
    }
}
