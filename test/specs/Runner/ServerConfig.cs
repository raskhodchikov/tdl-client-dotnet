namespace TDL.Test.Specs.Runner
{
    internal class ServerConfig
    {
        public string Verb { get; set; }
        public string EndpointEquals { get; set; }
        public string EndpointMatches { get; set; }
        public int Status { get; set; }
        public string ResponseBody { get; set; }
        public string AcceptHeader { get; set; }
        public string StatusMessage { get; set; }
    }

    internal class WiremockMapping
    {
        public WiremockMappingRequest Request { get; set; }
        public WiremockMappingResponse Response { get; set; }

        public WiremockMapping(ServerConfig config)
        {
            Request = new WiremockMappingRequest
            {
                UrlPattern = config.EndpointMatches,
                Url = config.EndpointEquals,
                Method = config.Verb
            };

            if (config.AcceptHeader != null)
            {
                Request.Header = new WiremockMappingHeader
                {
                    Accept = new WiremockMappingHeaderAccept
                    {
                        AcceptHeader = config.AcceptHeader
                    }
                };
            }

            Response = new WiremockMappingResponse
            {
                Body = config.ResponseBody,
                StatusMessage = config.StatusMessage,
                Status = config.Status
            };
        }
    }

    internal class WiremockMappingResponse
    {
        public string Body { get; set; }
        public string StatusMessage { get; set; }
        public int Status { get; set; }
    }

    internal class WiremockMappingRequest
    {
        public string UrlPattern { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public WiremockMappingHeader Header { get; set; }
    }

    internal class WiremockMappingHeader
    {
        public WiremockMappingHeaderAccept Accept { get; set; }
    }

    internal class WiremockMappingHeaderAccept
    {
        public string AcceptHeader { get; set; }
    }

    internal class MatchingDataRequest
    {
        public string UrlPattern { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public MatchingDataRequestBodyPattern[] BodyPatterns { get; set; }
    }

    internal class MatchingDataRequestBodyPattern
    {
        public string EqualTo { get; set; }
    }

    internal class MatchingDataResponse
    {
        public int Count { get; set; }
    }
}
