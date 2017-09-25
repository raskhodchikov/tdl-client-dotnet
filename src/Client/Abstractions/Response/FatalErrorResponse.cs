using TDL.Client.Actions;

namespace TDL.Client.Abstractions.Response
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
    }
}
