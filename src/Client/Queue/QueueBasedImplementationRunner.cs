using System;
using TDL.Client.Queue;
using TDL.Client.Queue.Abstractions;
using TDL.Client.Queue.Transport;
using TDL.Client.Utils;

namespace TDL.Client
{
    public partial class QueueBasedImplementationRunner
    {
        private readonly Audit audit;
        private readonly ProcessingRules deployProcessingRules;
        private readonly ImplementationRunnerConfig config;

        public QueueBasedImplementationRunner(
            ImplementationRunnerConfig config,
            ProcessingRules deployProcessingRules)
        {
            this.config = config;
            this.deployProcessingRules = deployProcessingRules;

            audit = new Audit(config.AuditStream);
        }

        public long RequestTimeoutMilliseconds => config.RequestTimeoutMilliseconds;

        public void Run()
        {
            audit.LogLine("Starting client");

            try
            {
                using (var remoteBroker = new RemoteBroker(
                    config.Hostname,
                    config.Port,
                    config.UniqueId,
                    config.RequestTimeoutMilliseconds))
                {
                    audit.LogLine("Waiting for requests");
                    var request = remoteBroker.Receive();
                    while (request.HasValue)
                    {
                        request = ApplyProcessingRules(request.Value, deployProcessingRules, remoteBroker);
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
