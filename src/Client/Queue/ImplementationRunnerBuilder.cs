using TDL.Client.Audit;

namespace TDL.Client.Queue
{
    public class ImplementationRunnerConfig
    {
        public string Hostname { get; private set; }
        public int Port { get; private set; }
        public string UniqueId { get; private set; }
        public long RequestTimeoutMilliseconds { get; private set; }
        public IAuditStream AuditStream { get; private set; }

        public ImplementationRunnerConfig()
        {
            Port = 61616;
            RequestTimeoutMilliseconds = 500;
        }

        public ImplementationRunnerConfig SetHostname(string hostname)
        {
            Hostname = hostname;
            return this;
        }

        public ImplementationRunnerConfig SetPort(int port)
        {
            Port = port;
            return this;
        }

        public ImplementationRunnerConfig SetUniqueId(string uniqueId)
        {
            UniqueId = uniqueId;
            return this;
        }

        public ImplementationRunnerConfig SetTimeToWaitForRequests(long timeToWaitForRequests)
        {
            RequestTimeoutMilliseconds = timeToWaitForRequests;
            return this;
        }

        public ImplementationRunnerConfig SetAuditStream(IAuditStream auditStream)
        {
            AuditStream = auditStream;
            return this;
        }
    }
}
