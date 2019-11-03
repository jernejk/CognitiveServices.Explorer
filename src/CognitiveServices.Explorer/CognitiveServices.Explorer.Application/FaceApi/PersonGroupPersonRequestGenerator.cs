using Microsoft.AspNetCore.Http;

namespace CognitiveServices.Explorer.Application.FaceApi
{
    public class PersonGroupPersonRequestGenerator
    {
        public static HttpRequest List(string groupId)
        {
            return new HttpRequest
            {
                HttpMethod = HttpMethods.Get,
                RelativePath = $"face/v1.0/persongroups/{groupId}/persons",
                Cost = ServiceCost.FaceApiTransaction(1),
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395241"
            };
        }

        public static HttpRequest Get(string groupId, string personId)
        {
            return new HttpRequest
            {
                HttpMethod = HttpMethods.Get,
                RelativePath = $"face/v1.0/persongroups/{groupId}/persons/{personId}",
                Cost = ServiceCost.FaceApiTransaction(1),
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f3039523f"
            };
        }
    }
}
