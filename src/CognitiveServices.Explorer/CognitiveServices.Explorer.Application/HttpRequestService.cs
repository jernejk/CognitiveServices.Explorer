using Flurl;
using Flurl.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application
{
    public class CurlRequestService
    {
        public async Task RunCurl(HttpRequest request, CognitiveServiceConfig cognitiveServiceConfig, CancellationToken token = default)
        {
            var url = new Url(cognitiveServiceConfig.BaseUrl)
                .AppendPathSegment(request.RelativePath)
                .WithHeader(request.TokenHeaderName, cognitiveServiceConfig.Token);

            Task<HttpResponseMessage> responseTask = null!;
            if (string.Equals(request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                responseTask = url.GetAsync(token);
            }
            else if (string.Equals(request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
            {
                if (request.BinaryContent != null)
                {
                    var content = new ByteArrayContent(request.BinaryContent);
                    content.Headers.ContentType = new MediaTypeHeaderValue(request.ContentType);
                    responseTask = url.PostAsync(content, token);
                }
                else
                {
                    var content = new StringContent(request.Body ?? string.Empty);
                    content.Headers.ContentType = new MediaTypeHeaderValue(request.ContentType);
                    responseTask = url.PostAsync(content, token);
                }
            }

            try
            {
                using var response = await responseTask;
                string json = await response.Content.ReadAsStringAsync();
            }
            catch
            {

            }
        }
    }
}
