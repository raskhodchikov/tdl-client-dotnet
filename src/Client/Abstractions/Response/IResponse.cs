using TDL.Client.Actions;
using TDL.Client.Audit;

namespace TDL.Client.Abstractions.Response
{
    public interface IResponse : IAuditable
    {
        string Id { get; }

        object Result { get; }

        IClientAction ClientAction { get; }
    }
}
