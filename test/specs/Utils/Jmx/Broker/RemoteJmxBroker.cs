using System.Collections.Generic;

namespace TDL.Test.Specs.Utils.Jmx.Broker
{
    internal class RemoteJmxQueue
    {
        private readonly JolokiaSession jolokiaSession;
        private readonly string queueBean;

        public RemoteJmxQueue(JolokiaSession jolokiaSession, string brokerName, string queueName)
        {
            this.jolokiaSession = jolokiaSession;
            queueBean = $"org.apache.activemq:type=Broker,brokerName={brokerName},destinationType=Queue,destinationName={queueName}";
        }

        public void SendTextMessage(string message)
        {
            jolokiaSession.Request(new Dictionary<string, object>
            {
                ["type"] = "exec",
                ["mbean"] = queueBean,
                ["operation"] = "sendTextMessage(java.lang.String)",
                ["arguments"] = new List<string> {message}
            });
        }

        public long GetSize()
        {
            var response = jolokiaSession.Request(new Dictionary<string, object>
            {
                ["type"] = "read",
                ["mbean"] = queueBean,
                ["attribute"] = "QueueSize"
            });

            return long.Parse(response.Value);
        }
    }
}
