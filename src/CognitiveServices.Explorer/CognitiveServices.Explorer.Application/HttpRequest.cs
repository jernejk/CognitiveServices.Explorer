using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CognitiveServices.Explorer.Application
{
    public class HttpRequest
    {
        public string? RequestName { get; set; }
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public string RelativePath { get; set; } = string.Empty;
        public Dictionary<string, string>? Queries { get; set; }
        public string HttpMethod { get; set; } = HttpMethods.Get;
        public string? Body { get; set; }
        public byte[]? BinaryContent { get; set; }
        public string ContentType { get; set; } = "application/json";
        public int ExpectedHtppCode { get; set; } = 200;
        public string TokenHeaderName { get; set; } = "Ocp-Apim-Subscription-Key";
        public ServiceCost? Cost { get; set; }
        public string? CognitiveServiceDoc { get; set; }
        public string? AbsoluteUrl { get; set; }
    }
}
