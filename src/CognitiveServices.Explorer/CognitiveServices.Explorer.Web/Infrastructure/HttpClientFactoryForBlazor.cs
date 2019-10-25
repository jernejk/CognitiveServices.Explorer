using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Blazor.Http;
using System.Net.Http;

namespace CognitiveServices.Explorer.Web.Infrastructure
{
    public class HttpClientFactoryForBlazor : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            return new WebAssemblyHttpMessageHandler();
        }
    }
}
