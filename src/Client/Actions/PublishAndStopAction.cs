using TDL.Client.Abstractions;
using TDL.Client.Abstractions.Response;
using TDL.Client.Transport;

namespace TDL.Client.Actions
{
    public class PublishAndStopAction : IClientAction
    {
        public void AfterResponse(RemoteBroker remoteBroker, Request request, IResponse response)
        {
            remoteBroker.RespondTo(request, response);
        }

        public Request GetNextRequest(RemoteBroker remoteBroker) => null;
    }
}
