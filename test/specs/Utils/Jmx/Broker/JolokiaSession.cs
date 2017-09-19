using System;
using System.Collections.Generic;
using System.Net;
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

        public JolokiaResponse Request(Dictionary<string, object> jolokiaPayload)
        {
            var request = new RestRequest(Method.POST) {JsonSerializer = NewtonsoftJsonSerializer.Default};
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(jolokiaPayload);

            var response = restClient.Execute<JolokiaResponse>(request).Data;
            return response;
        }
    }
}
