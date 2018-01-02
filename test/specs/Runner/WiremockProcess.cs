using RestSharp;

namespace TDL.Test.Specs.Runner
{
    internal class WiremockProcess
    {
        private readonly RestClient restClient;

        public WiremockProcess(string hostname, int port)
        {
            restClient = new RestClient($"http://{hostname}:{port}");
        }

        public void CreateNewMapping(ServerConfig config)
        {
            var request = new RestRequest("__admin/mappings/new", Method.POST);
            request.AddJsonBody(new WiremockMapping(config));

            restClient.Execute(request);
        }

        public void Reset()
        {
            var request = new RestRequest("__admin/reset", Method.POST);

            restClient.Execute(request);
        }

        public bool VerifyEndpointWasHit(string endpoint, string methodType, string body)
        {
            return CountRequestsWithEndpoint(endpoint, methodType, body) == 1;
        }

        private int CountRequestsWithEndpoint(string endpoint, string verb, string body)
        {
            var request = new RestRequest("__admin/requests/count", Method.POST);
            var requestData = new MatchingDataRequest
            {
                Url = endpoint,
                Method = verb
            };

            if (body != null)
            {
                requestData.BodyPatterns = new[]
                {
                    new MatchingDataRequestBodyPattern { EqualTo = body }
                };
            }

            var response = restClient.Execute<MatchingDataResponse>(request);
            return response.Data.Count;
        }
    }
}
