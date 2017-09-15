using System;
using System.Collections.Generic;
using RestSharp;

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

        public void Request(Dictionary<string, object> jolokiaPayload)
        {
            var client = new RestClient($"http://{Host}:{Port}/api/jolokia");

            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(jolokiaPayload);

            var response = client.Execute(request);
        }

        public void SendTextMessage(string message)
        {
            Request(new Dictionary<string, object>
            {
                ["type"] = "exec",
                ["mbean"] = "org.apache.activemq:type=Broker,brokerName=TEST.BROKER,destinationType=Queue,destinationName=TEST.QUEUE",
                ["operation"] = "sendTextMessage(java.lang.String)",
                ["arguments"] = new List<string> {message}
            });
        }
    }
}
