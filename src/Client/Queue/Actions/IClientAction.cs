using TDL.Client.Queue.Abstractions;
using TDL.Client.Queue.Abstractions.Response;
using TDL.Client.Audit;
using TDL.Client.Queue.Transport;
using TDL.Client.Utils;

namespace TDL.Client.Queue.Actions
{
    public interface IClientAction : IAuditable
    {
        void AfterResponse(RemoteBroker remoteBroker, Request request, IResponse response);

        Maybe<Request> GetNextRequest(RemoteBroker remoteBroker);
    }
}
