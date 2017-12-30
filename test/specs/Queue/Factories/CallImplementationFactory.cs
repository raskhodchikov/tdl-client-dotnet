using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TDL.Test.Specs.Queue.Factories
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
                    "ok",
                ["work for 600ms"] = args =>
                {
                    try
                    {
                        Thread.Sleep(600);
                    }
                    catch (ThreadInterruptedException ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    return "OK";
                }
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
