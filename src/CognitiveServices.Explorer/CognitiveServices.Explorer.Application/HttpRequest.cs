using System.Collections.Generic;

namespace CognitiveServices.Explorer.Application
{
    public class HttpRequest
    {
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public string RelativePath { get; set; } = string.Empty;
        public string HttpMethod { get; set; } = "GET";
        public string? Body { get; set; }
        public byte[]? BinaryContent { get; set; }
        public string ContentType { get; set; } = "application/json";
        public int ExpectedHtppCode { get; set; } = 200;
        public string TokenHeaderName { get; set; } = "Ocp-Apim-Subscription-Key";
        public string? CognitiveServiceDoc { get; set; }
    }
}
