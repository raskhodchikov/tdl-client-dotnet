using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using TDL.Test.Specs.Utils.Jmx.Broker.JolokiaResponses;

namespace TDL.Test.Specs.Utils.Jmx.Broker
{
    internal class JolokiaSession
    {
        private readonly RestClient restClient;

        private JolokiaSession(Uri jolokiaUri)
        {
            restClient = new RestClient(jolokiaUri);
        }

        public static JolokiaSession Connect(string host, int adminPort)
        {
            var jolokiaUri = new Uri($"http://{host}:{adminPort}/api/jolokia");

            return new JolokiaSession(jolokiaUri);
        }

        public JolokiaResponse<string> Request(Dictionary<string, object> jolokiaPayload)
        {
            return Request<string>(jolokiaPayload);
        }

        public JolokiaResponse<T> Request<T>(Dictionary<string, object> jolokiaPayload)
        {
            var request = new RestRequest(Method.POST)
            {
                RequestFormat = DataFormat.Json,
                OnBeforeDeserialization = r => { r.ContentType = MimeType.Json; }
            };
            request.AddHeader("Content-Type", MimeType.Json);
            request.AddJsonBody(jolokiaPayload);

            var response = restClient.Execute<JolokiaResponse<T>>(request);
            ValidateResponse(response.StatusCode, response.Content);
            ValidateResponse(response.Data.Status, $"{response.Data.ErrorType}: {response.Data.Error}");

            return response.Data;
        }

        private static void ValidateResponse(HttpStatusCode responseStatusCode, string content)
        {
            if (responseStatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Failed Jolokia call: {responseStatusCode}: {content}");
            }
        }
    }
}
