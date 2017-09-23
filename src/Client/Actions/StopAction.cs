namespace TDL.Client.Actions
{
    public class StopAction : IClientAction
    {
        public void AfterResponse(RemoteBroker remoteBroker, Request request, Response response)
        {
        }

        public Request GetNextRequest(RemoteBroker remoteBroker)
        {
            return null;
        }
    }
}
