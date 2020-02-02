using CognitiveServices.Explorer.Domain.Profiles;
using Flurl;
using Flurl.Http;
using MediatR;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace CognitiveServices.Explorer.Application.Commands
{
    public class ExecuteCognitiveServicesCommand : IRequest<string?>
    {
        public ExecuteCognitiveServicesCommand(HttpRequest request, CognitiveServiceConfig cognitiveServiceConfig)
        {
            Request = request;
            CognitiveServiceConfig = cognitiveServiceConfig;
        }

        public HttpRequest Request { get; }
        public CognitiveServiceConfig CognitiveServiceConfig { get; }

        public class Handler : IRequestHandler<ExecuteCognitiveServicesCommand, string?>
        {
            public async Task<string?> Handle(ExecuteCognitiveServicesCommand request, CancellationToken ct)
            {
                var url = new Url(request.CognitiveServiceConfig.BaseUrl)
                    .AppendPathSegment(request.Request.RelativePath)
                    .WithHeader(request.Request.TokenHeaderName, request.CognitiveServiceConfig.Token);

                if (request.Request.Queries?.Any() == true)
                {
                    url = url.SetQueryParams(request.Request.Queries);
                }

                Task<IFlurlResponse> responseTask = null!;
                string httpMethod = request.Request.HttpMethod.ToUpperInvariant();
                switch (httpMethod)
                {
                    case "GET":
                        responseTask = url.GetAsync(ct);
                        break;
                    case "POST":
                    case "PUT":
                        {
                            HttpContent content;
                            if (request.Request.BinaryContent != null)
                            {
                                var byteArrayContent = new ByteArrayContent(request.Request.BinaryContent);
                                byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(request.Request.ContentType);
                                content = byteArrayContent;
                            }
                            else
                            {
                                var stringContent = new StringContent(request.Request.Body ?? string.Empty);
                                stringContent.Headers.ContentType = new MediaTypeHeaderValue(request.Request.ContentType);
                                content = stringContent;
                            }

                            responseTask = httpMethod == "POST" ?
                                url.PostAsync(content, ct) :
                                url.PutAsync(content, ct);

                            break;
                        }
                    case "DELETE":
                        responseTask = url.DeleteAsync(ct);
                        break;
                    case "PATCH":
                        {
                            var content = new StringContent(request.Request.Body ?? string.Empty);
                            content.Headers.ContentType = new MediaTypeHeaderValue(request.Request.ContentType);
                            responseTask = url.PatchAsync(content, ct);
                        }
                        break;
                }

                using var response = await responseTask;
                return await response.GetStringAsync();
            }
        }
    }
}
