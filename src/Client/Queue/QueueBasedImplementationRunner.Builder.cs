using System;
using TDL.Client.Audit;
using TDL.Client.Queue;
using TDL.Client.Queue.Actions;

namespace TDL.Client
{
    public partial class QueueBasedImplementationRunner
    {
        public class Builder
        {
            private ProcessingRules deployProcessingRules;
            private ImplementationRunnerConfig config;

            public Builder()
            {
                deployProcessingRules = CreateDeployProcessingRules();
            }

            public Builder SetConfig(ImplementationRunnerConfig config)
            {
                this.config = config;
                return this;
            }

            public Builder WithSolutionFor(string methodName, Func<string[], object> userImplementation)
            {
                deployProcessingRules
                    .On(methodName)
                    .Call(userImplementation)
                    .Then(ClientActions.Publish);
                return this;
            }

            public Builder WithSolutionFor(string methodName, Func<string[], object> userImplementation, IClientAction clientAction)
            {
                deployProcessingRules
                    .On(methodName)
                    .Call(userImplementation)
                    .Then(clientAction);
                return this;
            }

            public QueueBasedImplementationRunner Create()
            {
                return new QueueBasedImplementationRunner(config, deployProcessingRules);
            }

            private static ProcessingRules CreateDeployProcessingRules()
            {
                var deployProcessingRules = new ProcessingRules();

                // Debt - we only need this to consume message from the server
                deployProcessingRules
                        .On("display_description")
                        .Call(p => "OK")
                        .Then(ClientActions.Publish);

                return deployProcessingRules;
            }
        }
    }
}
