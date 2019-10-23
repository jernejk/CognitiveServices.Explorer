using Newtonsoft.Json;
using System.Collections.Generic;

namespace CognitiveServices.Explorer.Application.FaceApi
{
    public class FaceCurlGenerator
    {
        public HttpRequest Detect(byte[] data)
        {
            return new HttpRequest
            {
                HttpMethod = "POST",
                BinaryContent = data,
                ContentType = "application/octet-stream",
                RelativePath = "face/v1.0/detect",
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236"
            };
        }

        public HttpRequest Detect(string url)
        {
            return new HttpRequest
            {
                HttpMethod = "POST",
                ContentType = "application/json",
                RelativePath = "face/v1.0/detect",
                Body = JsonConvert.SerializeObject(new { url }),
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236"
            };
        }

        public HttpRequest Identify(string personGroupId, IEnumerable<string> faceIds, int maxNumOfCandidatesReturned = 1, float? confidenceThreshold = 0.5f)
        {
            return new HttpRequest
            {
                HttpMethod = "POST",
                ContentType = "application/json",
                RelativePath = "face/v1.0/identify",
                Body = JsonConvert.SerializeObject(new
                {
                    personGroupId,
                    faceIds,
                    maxNumOfCandidatesReturned,
                    confidenceThreshold
                }),
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395239"
            };
        }
    }
}
