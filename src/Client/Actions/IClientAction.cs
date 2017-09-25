using TDL.Client.Abstractions;
using TDL.Client.Abstractions.Response;
using TDL.Client.Transport;
using TDL.Client.Utils;

namespace TDL.Client.Actions
{
    public interface IClientAction
    {
        void AfterResponse(RemoteBroker remoteBroker, Request request, IResponse response);

        Maybe<Request> GetNextRequest(RemoteBroker remoteBroker);
    }
}
