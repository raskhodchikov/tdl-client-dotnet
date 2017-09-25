using System.Linq;
using Apache.NMS;
using TDL.Client.Audit;

namespace TDL.Client.Abstractions
{
    public class Request : IAuditable
    {
        public ITextMessage TextMessage { get; set; }

        public string MethodName { get; set; }

        public string[] Params { get; set; }

        public string Id { get; set; }

        public string AuditText =>
            $"id = {Id}, req = {MethodName}({Params.ToArray<object>().ToDisplayableString()})";
    }
}
