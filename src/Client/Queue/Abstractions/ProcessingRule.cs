using System;
using TDL.Client.Queue.Actions;

namespace TDL.Client.Queue.Abstractions
{
    public class ProcessingRule
    {
        public Func<string[], object> UserImplementation { get; }
        public IClientAction ClientAction { get; }

        public ProcessingRule(
            Func<string[], object> userImplementation,
            IClientAction clientAction)
        {
            UserImplementation = userImplementation;
            ClientAction = clientAction;
        }
    }
}
