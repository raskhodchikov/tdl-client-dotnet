using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using RestSharp.Deserializers;
using TDL.Test.Specs.Utils.Jmx.Broker.JolokiaResponses;
using TDL.Test.Specs.Utils.Serializers;

namespace TDL.Test.Specs.Utils.Jmx.Broker
{
    internal class JolokiaSession
    {
        private readonly RestClient restClient;
        private readonly JsonDeserializer jsonDeserializer;

        private JolokiaSession(Uri jolokiaUri)
        {
            restClient = new RestClient(jolokiaUri);
            jsonDeserializer = new JsonDeserializer();
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
                JsonSerializer = NewtonsoftJsonSerializer.Default,
                OnBeforeDeserialization = r => { r.ContentType = MimeType.Json; }
            };
            request.AddHeader("Content-Type", MimeType.Json);
            request.AddJsonBody(jolokiaPayload);

            var rawResponse = restClient.Execute(request);
            ValidateResponse(rawResponse.StatusCode, rawResponse.Content);

            var response = jsonDeserializer.Deserialize<JolokiaResponse<T>>(rawResponse);
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
