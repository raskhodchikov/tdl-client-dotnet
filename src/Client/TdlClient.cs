using System;
using TDL.Client.Abstractions;
using TDL.Client.Audit;
using TDL.Client.Transport;
using TDL.Client.Utils;

namespace TDL.Client
{
    public partial class TdlClient
    {
        private readonly string hostname;
        private readonly int port;
        private readonly string uniqueId;
        private readonly Audit audit;

        public long TimeToWaitForRequests { get; private set; }

        public TdlClient(
            string hostname,
            int port,
            string uniqueId,
            long timeToWaitForRequests,
            IAuditStream auditStream)
        {
            this.hostname = hostname;
            this.port = port;
            this.uniqueId = uniqueId;
            audit = new Audit(auditStream);

            TimeToWaitForRequests = timeToWaitForRequests;
        }

        public static Builder Build() => new Builder();

        public void GoLiveWith(ProcessingRules processingRules)
        {
            audit.LogLine("Starting client");

            try
            {
                using (var remoteBroker = new RemoteBroker(hostname, port, uniqueId, TimeToWaitForRequests))
                {
                    audit.LogLine("Waiting for requests");
                    var request = remoteBroker.Receive();
                    while (request.HasValue)
                    {
                        request = ApplyProcessingRules(request.Value, processingRules, remoteBroker);
                    }
                }
            }
            catch (Exception ex)
            {
                audit.LogException("There was a problem processing messages", ex);
            }

            audit.LogLine("Stopping client");
        }

        private Maybe<Request> ApplyProcessingRules(
            Request request,
            ProcessingRules processingRules,
            RemoteBroker remoteBroker)
        {
            audit.StartLine();
            audit.Log(request);

            var response = processingRules.GetResponseFor(request);
            audit.Log(response);

            var clientAction = response.ClientAction;
            audit.Log(clientAction);

            audit.EndLine();

            clientAction.AfterResponse(remoteBroker, request, response);

            return clientAction.GetNextRequest(remoteBroker);
        }
    }
}
