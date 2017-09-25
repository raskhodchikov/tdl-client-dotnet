using System;
using System.Collections.Generic;
using TDL.Client.Abstractions;
using TDL.Client.Abstractions.Response;
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

        public IResponse GetResponseFor(Request request)
        {
            if (!rules.ContainsKey(request.MethodName))
                return new FatalErrorResponse($"method '{request.MethodName}' did not match any processing rule");

            var processingRule = rules[request.MethodName];

            try
            {
                var result = processingRule.UserImplementation(request.Params);
                return new ValidResponse(request.Id, result, processingRule.ClientAction);
            }
            catch (Exception)
            {
                return new FatalErrorResponse("user implementation raised exception");
            }
        }
    }
}
