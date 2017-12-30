using TDL.Client.Queue.Actions;
using TDL.Client.Audit;

namespace TDL.Client.Queue.Abstractions.Response
{
    public interface IResponse : IAuditable
    {
        string Id { get; }

        object Result { get; }

        IClientAction ClientAction { get; }
    }
}
