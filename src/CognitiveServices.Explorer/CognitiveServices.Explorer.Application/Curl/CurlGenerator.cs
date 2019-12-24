using CognitiveServices.Explorer.Domain.Profiles;
using System.Linq;

namespace CognitiveServices.Explorer.Application.Curl
{
    public class CurlGenerator
    {
        public string Generate(HttpRequest request, CognitiveServiceConfig? cognitiveServiceConfig, bool showToken = false)
        {
            var url = $"{cognitiveServiceConfig?.BaseUrl}/{request.RelativePath}";
            if (request.Queries?.Any() == true)
            {
                url += "?";
                url += string.Join("&", request.Queries.Select(q => $"{q.Key}={q.Value}"));
            }

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

            if (!string.IsNullOrWhiteSpace(request.Body))
            {
                curl += $"\n  -d '{request.Body!.Replace("'", "\\'")}'";
            }

            return curl;
        }

        public string GetName(HttpRequest request)
        {
            return $"{request.HttpMethod.ToUpperInvariant()} {request.RelativePath}";
        }
    }
}
