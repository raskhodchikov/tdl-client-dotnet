using TDL.Client.Queue.Actions;
using TDL.Client.Audit;

namespace TDL.Client.Queue.Abstractions.Response
{
    public class ValidResponse : IResponse
    {
        public string Id { get; set; }

        public object Result { get; }

        public IClientAction ClientAction { get; }

        public ValidResponse(
            string id,
            object result,
            IClientAction clientAction)
        {
            Id = id;
            Result = result;
            ClientAction = clientAction;
        }

        public string AuditText => $"resp = {Result?.ToDisplayableString()}";
    }
}
