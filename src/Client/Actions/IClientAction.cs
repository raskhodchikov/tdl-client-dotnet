namespace TDL.Client.Actions
{
    public interface IClientAction
    {
        void AfterResponse(RemoteBroker remoteBroker, Request request, Response response);

        Request GetNextRequest(RemoteBroker remoteBroker);
    }
}
