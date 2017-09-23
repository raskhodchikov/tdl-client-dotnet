namespace TDL.Client.Actions
{
    public class PublishAction : IClientAction
    {
        public void AfterResponse(RemoteBroker remoteBroker, Request request, Response response)
        {
            remoteBroker.RespondTo(request, response);
        }

        public Request GetNextRequest(RemoteBroker remoteBroker)
        {
            return remoteBroker.Resolve();
        }
    }
}
