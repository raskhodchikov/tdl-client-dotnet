using System;
using System.Collections.Generic;
using RestSharp;

namespace TDL.Test.Specs.Utils.Jmx.Broker
{
    internal class JolokiaSession
    {
        private readonly RestClient restClient;

        public JolokiaSession(Uri jolokiaUri)
        {
            restClient = new RestClient(jolokiaUri);
        }

        public static JolokiaSession Connect(string host, int adminPort)
        {
            var jolokiaUri = new Uri($"http://{host}:{adminPort}/api/jolokia");

            return new JolokiaSession(jolokiaUri);
        }

        public object Request(Dictionary<string, object> jolokiaPayload)
        {
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(jolokiaPayload);

            var response = restClient.Execute(request).Content;
            return response;
        }
    }
}
