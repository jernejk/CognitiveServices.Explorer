using System.Collections.Generic;

namespace CognitiveServices.Explorer.Application.FaceApi
{
    public class PersonGroupPersonRequestGenerator
    {
        public static HttpRequest List(string groupId)
        {
            return new HttpRequest
            {
                HttpMethod = "GET",
                RelativePath = $"face/v1.0/persongroups/{groupId}/persons",
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395241"
            };
        }
    }
}
