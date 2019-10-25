namespace CognitiveServices.Explorer.Application
{
    public class CognitiveServiceConfig
    {
        public CognitiveServiceConfig() { }

        public CognitiveServiceConfig(string serviceName, string baseUrl, string token)
        {
            ServiceName = serviceName;
            BaseUrl = baseUrl;
            Token = token;
        }

        public string ServiceName { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
