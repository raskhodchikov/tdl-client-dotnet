using TDL.Client.Audit;

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
            private IAuditStream auditStream;

            public Builder()
            {
                port = 61616;
                timeToWaitForRequests = 500;
            }

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

            public Builder SetAuditStream(IAuditStream auditStream)
            {
                this.auditStream = auditStream;
                return this;
            }

            public TdlClient Create() => new TdlClient(hostname, port, uniqueId, timeToWaitForRequests, auditStream);
        }
    }
}
