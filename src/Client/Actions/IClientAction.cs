using TDL.Client.Abstractions;
using TDL.Client.Abstractions.Response;
using TDL.Client.Audit;
using TDL.Client.Transport;
using TDL.Client.Utils;

namespace TDL.Client.Actions
{
    public interface IClientAction : IAuditable
    {
        void AfterResponse(RemoteBroker remoteBroker, Request request, IResponse response);

        Maybe<Request> GetNextRequest(RemoteBroker remoteBroker);
    }
}
