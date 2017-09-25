using Apache.NMS;

namespace TDL.Client.Abstractions
{
    public class Request
    {
        public ITextMessage TextMessage { get; set; }

        public string MethodName { get; set; }

        public string[] Params { get; set; }

        public string Id { get; set; }
    }
}
