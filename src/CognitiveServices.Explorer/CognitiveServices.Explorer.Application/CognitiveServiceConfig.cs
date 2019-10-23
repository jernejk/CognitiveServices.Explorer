namespace CognitiveServices.Explorer.Application
{
    public class CognitiveServiceConfig
    {
        public CognitiveServiceConfig(string baseUrl, string token)
        {
            BaseUrl = baseUrl;
            Token = token;
        }

        public string BaseUrl { get; set; }

        public string Token { get; set; }
    }
}
