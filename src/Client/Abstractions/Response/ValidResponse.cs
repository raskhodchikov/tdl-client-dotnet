using TDL.Client.Actions;

namespace TDL.Client.Abstractions.Response
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
    }
}
