namespace TDL.Client
{
    public partial class TdlClient
    {
        private string hostname;
        private int port;
        private string uniqueId;
        private long timeToWaitForRequests;

        public TdlClient(
            string hostname,
            int port,
            string uniqueId,
            long timeToWaitForRequests)
        {
            this.hostname = hostname;
            this.port = port;
            this.uniqueId = uniqueId;
            this.timeToWaitForRequests = timeToWaitForRequests;
        }

        public static Builder Build() => new Builder();

        public void GoLiveWith(ProcessingRules processingRules)
        {
            throw new System.NotImplementedException();
        }
    }
}
