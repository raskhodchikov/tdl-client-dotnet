namespace TDL.Client.Actions
{
    public class PublishAndStopAction : IClientAction
    {
        public void AfterResponse(RemoteBroker remoteBroker, Request request, Response response)
        {
            remoteBroker.RespondTo(request, response);
        }

        public Request GetNextRequest(RemoteBroker remoteBroker)
        {
            return null;
        }
    }
}
