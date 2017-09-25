using System;
using System.Collections.Generic;
using TDL.Client.Actions;

namespace TDL.Test.Specs
{
    internal class ClientActionsFactory
    {
        private static readonly Dictionary<string, IClientAction> Actions =
            new Dictionary<string, IClientAction>
            {
                ["publish"] = ClientActions.Publish,
                ["stop"] = ClientActions.Stop,
                ["publish and stop"] = ClientActions.PublishAndStop
            };

        public static IClientAction Get(string action)
        {
            if (!Actions.ContainsKey(action))
            {
                throw new ArgumentException($@"Not a valid action reference: ""{action}""");
            }

            return Actions[action];
        }
    }
}
