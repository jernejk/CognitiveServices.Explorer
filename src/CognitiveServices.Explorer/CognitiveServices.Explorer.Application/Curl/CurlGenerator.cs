namespace CognitiveServices.Explorer.Application.Curl
{
    public class CurlGenerator
    {
        public string Generate(HttpRequest request, CognitiveServiceConfig? cognitiveServiceConfig, bool showToken = false)
        {
            var url = $"{cognitiveServiceConfig?.BaseUrl}/{request.RelativePath}";
            var curl = $"curl -X {request.HttpMethod.ToUpperInvariant()} \\";
            curl += $"\n  '{url}' \\";
            if (!string.IsNullOrWhiteSpace(request.ContentType))
            {
                curl += $"\n  -H 'Content-Type: {request.ContentType}' \\";
            }

            string token = showToken && cognitiveServiceConfig != null
                ? cognitiveServiceConfig.Token
                : "***";

            curl += $"\n  -H '{request.TokenHeaderName}: {token}'";

            return curl;
        }

        public string GetName(HttpRequest request)
        {
            return $"{request.HttpMethod.ToUpperInvariant()} {request.RelativePath}";
        }
    }
}
