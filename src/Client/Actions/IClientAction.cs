using TDL.Client.Abstractions;
using TDL.Client.Abstractions.Response;
using TDL.Client.Transport;

namespace TDL.Client.Actions
{
    public interface IClientAction
    {
        void AfterResponse(RemoteBroker remoteBroker, Request request, IResponse response);

        Request GetNextRequest(RemoteBroker remoteBroker);
    }
}
