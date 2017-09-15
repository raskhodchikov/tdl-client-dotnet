using System;
using TDL.Client;

namespace TDL.Demo
{
    internal class Program
    {
        private static void Main()
        {
            var tdlClient = new TdlClient(
                "localhost",
                28161,
                "testuser@example.com",
                TimeSpan.Zero);

            tdlClient.SendTextMessage("test");
        }
    }
}
