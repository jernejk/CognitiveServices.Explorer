using Flurl;
using Flurl.Http;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application
{
    public class HttpRequestService
    {
        public async Task<string?> Send(HttpRequest request, CognitiveServiceConfig cognitiveServiceConfig, CancellationToken token = default)
        {
            var url = new Url(cognitiveServiceConfig.BaseUrl)
                .AppendPathSegment(request.RelativePath)
                .WithHeader(request.TokenHeaderName, cognitiveServiceConfig.Token);

            if (request.Queries?.Any() == true)
            {
                url = url.SetQueryParams(request.Queries);
            }

            Task<HttpResponseMessage> responseTask = null!;
            string httpMethod = request.HttpMethod.ToUpperInvariant();
            switch (httpMethod)
            {
                case "GET":
                    responseTask = url.GetAsync(token);
                    break;
                case "POST":
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

                        break;
                    }
                case "DELETE":
                    responseTask = url.DeleteAsync(token);
                    break;
                case "PATCH":
                    {
                        var content = new StringContent(request.Body ?? string.Empty);
                        content.Headers.ContentType = new MediaTypeHeaderValue(request.ContentType);
                        responseTask = url.PatchAsync(content, token);
                    }
                    break;
            }

            try
            {
                using var response = await responseTask;
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
                return null;
            }
        }
    }
}
