namespace CognitiveServices.Explorer.Application
{
    public class CognitiveServiceConfig
    {
        public CognitiveServiceConfig() { }

        public CognitiveServiceConfig(string serviceName)
        {
            ServiceName = serviceName;
        }

        public CognitiveServiceConfig(string serviceName, string profileName, string baseUrl, string token)
        {
            ServiceName = serviceName;
            ProfileName = profileName;
            BaseUrl = baseUrl;
            Token = token;
        }

        public string? ProfileName { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
