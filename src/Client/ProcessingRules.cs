using System;
using System.Collections.Generic;
using TDL.Client.Abstractions;
using TDL.Client.Actions;

namespace TDL.Client
{
    public partial class ProcessingRules
    {
        private readonly Dictionary<string, ProcessingRule> rules = new Dictionary<string, ProcessingRule>();

        public Builder On(string methodName) => new Builder(methodName, this);

        private void Add(
            string methodName,
            Func<string[], object> userImplementation,
            IClientAction clientAction)
        {
            rules.Add(methodName, new ProcessingRule(userImplementation, clientAction));
        }
    }
}
