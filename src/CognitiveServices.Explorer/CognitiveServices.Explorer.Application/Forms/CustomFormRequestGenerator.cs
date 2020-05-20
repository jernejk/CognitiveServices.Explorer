using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Text.Json;

namespace CognitiveServices.Explorer.Application.Forms
{
    public class CustomFormRequestGenerator
    {
        public static HttpRequest GetModels(bool showSummary = true)
        {
            return new HttpRequest
            {
                HttpMethod = HttpMethods.Get,
                ContentType = "application/json",
                RelativePath = "formrecognizer/v2.0-preview/custom/models",
                Cost = ServiceCost.FormApiTransaction(1),
                Queries = new Dictionary<string, string> { { "op", showSummary.ToString() } },
                CognitiveServiceDoc = "https://westus2.dev.cognitive.microsoft.com/docs/services/form-recognizer-api-v2-preview/operations/GetCustomModels"
            };
        }

        public static HttpRequest StartAnalyzeForm(string modelId, byte[] data, string contentType, bool includeTextDetails = true)
        {
            // Content type should be image/jpeg, image/png, image/tiff or application/pdf.
            return new HttpRequest
            {
                HttpMethod = HttpMethods.Post,
                BinaryContent = data,
                ContentType = contentType,
                RelativePath = $"formrecognizer/v2.0-preview/custom/models/{modelId}/analyze",
                Queries = new Dictionary<string, string> { { "includeTextDetails", includeTextDetails.ToString() } },
                Cost = ServiceCost.FormApiTransaction(1),
                CognitiveServiceDoc = "https://westus2.dev.cognitive.microsoft.com/docs/services/form-recognizer-api-v2-preview/operations/AnalyzeWithCustomForm"
            };
        }

        public static HttpRequest StartAnalyzeForm(string modelId, string imageUrl, bool includeTextDetails = true)
        {
            return new HttpRequest
            {
                HttpMethod = HttpMethods.Post,
                Body = JsonSerializer.Serialize(new { source = imageUrl }),
                ContentType = "application/json",
                RelativePath = $"formrecognizer/v2.0-preview/custom/models/{modelId}/analyze",
                Queries = new Dictionary<string, string> { { "includeTextDetails", includeTextDetails.ToString() } },
                Cost = ServiceCost.FormApiTransaction(1),
                CognitiveServiceDoc = "https://westus2.dev.cognitive.microsoft.com/docs/services/form-recognizer-api-v2-preview/operations/AnalyzeWithCustomForm"
            };
        }

        public static HttpRequest GetResultFromForm(string operationLocation)
        {
            return new HttpRequest
            {
                HttpMethod = HttpMethods.Get,
                Cost = ServiceCost.FormApiTransaction(0),
                AbsoluteUrl = operationLocation,
                CognitiveServiceDoc = "https://westus2.dev.cognitive.microsoft.com/docs/services/form-recognizer-api-v2-preview/operations/GetAnalyzeFormResult"
            };
        }
    }
}
