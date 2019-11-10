using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

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

        public static HttpRequest Create(string groupId, string name, string? userData = null)
        {
            return new HttpRequest
            {
                HttpMethod = HttpMethods.Post,
                RelativePath = $"face/v1.0/persongroups/{groupId}/persons",
                Body = JsonConvert.SerializeObject(new
                {
                    name,
                    userData
                }),
                Cost = ServiceCost.FaceApiTransaction(1),
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f3039523c"
            };
        }

        public static HttpRequest Update(string groupId, string userId, string name, string? userData = null)
        {
            return new HttpRequest
            {
                HttpMethod = HttpMethods.Patch,
                RelativePath = $"face/v1.0/persongroups/{groupId}/persons/{userId}",
                Body = JsonConvert.SerializeObject(new
                {
                    name,
                    userData
                }),
                Cost = ServiceCost.FaceApiTransaction(1),
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395242"
            };
        }

        public static HttpRequest Delete(string groupId, string userId)
        {
            return new HttpRequest
            {
                HttpMethod = HttpMethods.Delete,
                RelativePath = $"face/v1.0/persongroups/{groupId}/persons/{userId}",
                Cost = ServiceCost.FaceApiTransaction(1),
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f3039523d"
            };
        }
    }
}
