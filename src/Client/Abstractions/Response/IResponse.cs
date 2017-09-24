using TDL.Client.Actions;

namespace TDL.Client.Abstractions.Response
{
    public interface IResponse
    {
        string Id { get; }

        object Result { get; }

        IClientAction ClientAction { get; }
    }
}
