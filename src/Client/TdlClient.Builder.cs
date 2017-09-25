namespace TDL.Client
{
    public partial class TdlClient
    {
        public class Builder
        {
            private string hostname;
            private int port;
            private string uniqueId;
            private long timeToWaitForRequests;

            public Builder SetHostname(string hostname)
            {
                this.hostname = hostname;
                return this;
            }

            public Builder SetPort(int port)
            {
                this.port = port;
                return this;
            }

            public Builder SetUniqueId(string uniqueId)
            {
                this.uniqueId = uniqueId;
                return this;
            }

            public Builder SetTimeToWaitForRequests(long timeToWaitForRequests)
            {
                this.timeToWaitForRequests = timeToWaitForRequests;
                return this;
            }

            public TdlClient Create() => new TdlClient(hostname, port, uniqueId, timeToWaitForRequests);
        }
    }
}
