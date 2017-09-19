using System;

namespace TDL.Client
{
    public class TdlClient
    {
        protected string Host;
        protected int Port;
        protected string UniqueId;
        protected TimeSpan TimeToWaitForRequests;


        public TdlClient(string host, int port, string uniqueId, TimeSpan timeToWaitForRequests)
        {
            Host = host;
            Port = port;
            UniqueId = uniqueId;
            TimeToWaitForRequests = timeToWaitForRequests;
        }
    }
}
