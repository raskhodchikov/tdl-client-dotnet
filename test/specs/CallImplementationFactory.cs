using System;
using System.Collections.Generic;
using System.Linq;

namespace TDL.Test.Specs
{
    internal static class CallImplementationFactory
    {
        private static readonly Dictionary<string, Func<string[], object>> CallImplementations =
            new Dictionary<string, Func<string[], object>>
            {
                ["add two numbers"] = args =>
                    args.Take(2).Sum(int.Parse),
                ["increment number"] = args =>
                    int.Parse(args[0]) + 1,
                ["return null"] = args =>
                    null,
                ["throw exception"] = args =>
                    throw new InvalidOperationException("faulty user code"),
                ["echo the request"] = args =>
                    args[0],
                ["some logic"] = args =>
                    "ok"
            };

        public static Func<string[], object> Get(string call)
        {
            if (!CallImplementations.ContainsKey(call))
            {
                throw new ArgumentException($@"Not a valid implementation reference: ""{call}"");");
            }

            return CallImplementations[call];
        }
    }
}
