using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using RestSharp.Deserializers;

namespace TDL.Test.Specs.Utils.Jmx.Broker
{
    internal class JolokiaSession
    {
        private readonly RestClient restClient;
        private readonly JsonDeserializer jsonDeserializer;

        public JolokiaSession(Uri jolokiaUri)
        {
            restClient = new RestClient(jolokiaUri);
            jsonDeserializer = new JsonDeserializer();
        }

        public static JolokiaSession Connect(string host, int adminPort)
        {
            var jolokiaUri = new Uri($"http://{host}:{adminPort}/api/jolokia");

            return new JolokiaSession(jolokiaUri);
        }

        public JolokiaResponse Request(Dictionary<string, object> jolokiaPayload)
        {
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(jolokiaPayload);

            var rawResponse = restClient.Execute(request);
            ValidateResponse(rawResponse.StatusCode, rawResponse.Content);

            var response = jsonDeserializer.Deserialize<JolokiaResponse>(rawResponse);
            ValidateResponse(response.Status, $"{response.ErrorType} {response.Error}");

            return response;
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
