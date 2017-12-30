using System;
using TDL.Client.Queue.Actions;

namespace TDL.Client.Queue
{
    public partial class ProcessingRules
    {
        public class Builder
        {
            private readonly ProcessingRules processingRules;
            private readonly string methodName;

            private Func<string[], object> userImplementation;

            public Builder(string methodName, ProcessingRules processingRules)
            {
                this.methodName = methodName;
                this.processingRules = processingRules;
            }

            public Builder Call(Func<string[], object> userImplementation)
            {
                this.userImplementation = userImplementation;
                return this;
            }

            public void Then(IClientAction clientAction)
            {
                processingRules.Add(methodName, userImplementation, clientAction);
            }
        }
    }
}
