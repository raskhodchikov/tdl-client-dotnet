using TDL.Client.Queue.Actions;

namespace TDL.Client.Queue.Abstractions.Response
{
    public class FatalErrorResponse : IResponse
    {
        public string Id => "error";

        public object Result { get; }

        public IClientAction ClientAction => ClientActions.Stop;

        public FatalErrorResponse(string message)
        {
            Result = message;
        }

        public string AuditText => $@"{Id} = ""{Result}""";
    }
}
