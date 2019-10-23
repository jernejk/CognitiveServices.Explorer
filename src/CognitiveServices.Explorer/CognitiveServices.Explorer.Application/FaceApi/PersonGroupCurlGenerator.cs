using Newtonsoft.Json;
using System.Collections.Generic;

namespace CognitiveServices.Explorer.Application.FaceApi
{
    public class PersonGroupCurlGenerator
    {
        public HttpRequest Create(string groupId, string name, string? userData = null, string? recognitionModel = null)
        {
            return new HttpRequest
            {
                HttpMethod = "POST",
                ContentType = "application/json",
                RelativePath = $"face/v1.0/persongroups/{groupId}",
                Body = JsonConvert.SerializeObject(new
                {
                    name,
                    recognitionModel,
                    userData
                }),
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395244"
            };
        }

        public HttpRequest Update(string groupId, string name, string? userData = null)
        {
            return new HttpRequest
            {
                HttpMethod = "PATCH",
                ContentType = "application/json",
                RelativePath = $"face/v1.0/persongroups/{groupId}",
                Body = JsonConvert.SerializeObject(new
                {
                    name,
                    userData
                }),
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f3039524a"
            };
        }

        public HttpRequest List()
        {
            return new HttpRequest
            {
                HttpMethod = "GET",
                RelativePath = $"face/v1.0/persongroups",
                Queries = new Dictionary<string, string>
                {
                    { "returnRecognitionModel", "true" }
                },
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395248"
            };
        }

        public HttpRequest Delete(string groupId)
        {
            return new HttpRequest
            {
                HttpMethod = "DELETE",
                RelativePath = $"face/v1.0/persongroups/{groupId}",
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395245"
            };
        }

        public HttpRequest Train(string groupId)
        {
            return new HttpRequest
            {
                HttpMethod = "POST",
                RelativePath = $"face/v1.0/persongroups/{groupId}/train",
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395249"
            };
        }

        public HttpRequest CheckTraining(string groupId)
        {
            return new HttpRequest
            {
                HttpMethod = "GET",
                RelativePath = $"face/v1.0/persongroups/{groupId}/training",
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395247"
            };
        }
    }
}
