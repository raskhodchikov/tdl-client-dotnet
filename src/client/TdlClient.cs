using TDL.Client.Abstractions;
using TDL.Client.Transport;

namespace TDL.Client
{
    public partial class TdlClient
    {
        private readonly string hostname;
        private readonly int port;
        private readonly string uniqueId;
        private readonly long timeToWaitForRequests;

        public TdlClient(
            string hostname,
            int port,
            string uniqueId,
            long timeToWaitForRequests)
        {
            this.hostname = hostname;
            this.port = port;
            this.uniqueId = uniqueId;
            this.timeToWaitForRequests = timeToWaitForRequests;
        }

        public static Builder Build() => new Builder();

        public void GoLiveWith(ProcessingRules processingRules)
        {
            using (var remoteBroker = new RemoteBroker(hostname, port, uniqueId, timeToWaitForRequests))
            {
                var request = remoteBroker.Recieve();
                while (request != null)
                {
                    request = ApplyProcessingRules(request, processingRules, remoteBroker);
                }
            }
        }

        private static Request ApplyProcessingRules(
            Request request,
            ProcessingRules processingRules,
            RemoteBroker remoteBroker)
        {
            var response = processingRules.GetResponseFor(request);

            var clientAction = response.ClientAction;

            clientAction?.AfterResponse(remoteBroker, request, response);

            return clientAction?.GetNextRequest(remoteBroker);
        }
    }
}
