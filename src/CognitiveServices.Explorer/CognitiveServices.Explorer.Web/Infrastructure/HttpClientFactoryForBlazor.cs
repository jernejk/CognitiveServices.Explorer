using Flurl.Http.Configuration;
using System;
using System.Net.Http;
using System.Reflection;

namespace CognitiveServices.Explorer.Web.Infrastructure
{
    public class HttpClientFactoryForBlazor : DefaultHttpClientFactory
    {
        public override HttpMessageHandler CreateMessageHandler()
        {
            var wasmHttpMessageHandlerType = Assembly.Load("WebAssembly.Net.Http").GetType("WebAssembly.Net.Http.HttpClient.WasmHttpMessageHandler");
            return (HttpMessageHandler)Activator.CreateInstance(wasmHttpMessageHandlerType);
        }
    }
}
