using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.Json;

namespace CognitiveServices.Explorer.Application.Forms
{
    public class CustomFormRequestGenerator
    {
        public const string StableVersion = "2.0";
        public const string PreviewVersion = "2.1-preview.1";

        public static HttpRequest GetModels(bool showSummary = true, string version = StableVersion)
        {
            return new HttpRequest
            {
                HttpMethod = HttpMethods.Get,
                ContentType = "application/json",
                RelativePath = $"formrecognizer/{version}/custom/models",
                Cost = ServiceCost.FormApiTransaction(1),
                Queries = new Dictionary<string, string> { { "op", showSummary.ToString() } },
                CognitiveServiceDoc = version switch
                {
                    StableVersion => "https://westus2.dev.cognitive.microsoft.com/docs/services/form-recognizer-api-v2/operations/GetCustomModels",
                    PreviewVersion => "https://westcentralus.dev.cognitive.microsoft.com/docs/services/form-recognizer-api-v2-1-preview-1/operations/GetCustomModels",
                    _ => ""
                }
            };
        }

        public static HttpRequest StartAnalyzeForm(string modelId, byte[] data, string contentType, bool includeTextDetails = true, string version = StableVersion)
        {
            // Content type should be image/jpeg, image/png, image/tiff or application/pdf.
            return new HttpRequest
            {
                HttpMethod = HttpMethods.Post,
                BinaryContent = data,
                ContentType = contentType,
                RelativePath = $"formrecognizer/{version}/custom/models/{modelId}/analyze",
                Queries = new Dictionary<string, string> { { "includeTextDetails", includeTextDetails.ToString() } },
                Cost = ServiceCost.FormApiTransaction(1),
                CognitiveServiceDoc = version switch
                {
                    StableVersion => "https://westus2.dev.cognitive.microsoft.com/docs/services/form-recognizer-api-v2/operations/AnalyzeWithCustomForm",
                    PreviewVersion => "https://westcentralus.dev.cognitive.microsoft.com/docs/services/form-recognizer-api-v2-1-preview-1/operations/GetAnalyzeReceiptResult",
                    _ => ""
                }
            };
        }

        public static HttpRequest StartAnalyzeForm(string modelId, string imageUrl, bool includeTextDetails = true, string version = StableVersion)
        {
            return new HttpRequest
            {
                HttpMethod = HttpMethods.Post,
                Body = JsonSerializer.Serialize(new { source = imageUrl }),
                ContentType = "application/json",
                RelativePath = $"formrecognizer/{version}/custom/models/{modelId}/analyze",
                Queries = new Dictionary<string, string> { { "includeTextDetails", includeTextDetails.ToString() } },
                Cost = ServiceCost.FormApiTransaction(1),
                CognitiveServiceDoc = version switch
                {
                    StableVersion => "https://westus2.dev.cognitive.microsoft.com/docs/services/form-recognizer-api-v2/operations/AnalyzeWithCustomForm",
                    PreviewVersion=> "https://westcentralus.dev.cognitive.microsoft.com/docs/services/form-recognizer-api-v2-1-preview-1/operations/GetAnalyzeReceiptResult",
                    _ => ""
                }
            };
        }

        public static HttpRequest GetResultFromForm(string operationLocation, string version = StableVersion)
        {
            return new HttpRequest
            {
                HttpMethod = HttpMethods.Get,
                Cost = ServiceCost.FormApiTransaction(0),
                AbsoluteUrl = operationLocation,
                CognitiveServiceDoc = version switch
                {
                    StableVersion => "https://westus2.dev.cognitive.microsoft.com/docs/services/form-recognizer-api-v2/operations/GetAnalyzeFormResult",
                    PreviewVersion => "https://westcentralus.dev.cognitive.microsoft.com/docs/services/form-recognizer-api-v2-1-preview-1/operations/GetAnalyzeFormResult",
                    _ => ""
                }
            };
        }
    }
}
