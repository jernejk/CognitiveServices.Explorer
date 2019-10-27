using Newtonsoft.Json;
using System.Collections.Generic;

namespace CognitiveServices.Explorer.Application.FaceApi
{
    public static class FaceRequestGenerator
    {
        public const string DefaultRecognitionModel = "recognition_01";
        public const string DefaultDetectionModel = "detection_01";

        public static HttpRequest Detect(
            byte[] data,
            string? returnFaceAttributes = null,
            bool? returnFaceLandmarks = null,
            string? recognitionModel = DefaultRecognitionModel,
            string? detectionModel = DefaultDetectionModel)
        {
            string additionalArguments = GenerateDetectRequestArguments(returnFaceAttributes, returnFaceLandmarks, recognitionModel, detectionModel);

            return new HttpRequest
            {
                HttpMethod = "POST",
                BinaryContent = data,
                ContentType = "application/octet-stream",
                RelativePath = $"face/v1.0/detect{additionalArguments}",
                Cost = ServiceCost.FaceApiTransation(returnFaceAttributes == null ? 1 : 2),
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236"
            };
        }

        public static HttpRequest Detect(string url,
            string? returnFaceAttributes = null,
            bool? returnFaceLandmarks = null,
            string? recognitionModel = DefaultRecognitionModel,
            string? detectionModel = DefaultDetectionModel)
        {
            string additionalArguments = GenerateDetectRequestArguments(returnFaceAttributes, returnFaceLandmarks, recognitionModel, detectionModel);

            return new HttpRequest
            {
                HttpMethod = "POST",
                ContentType = "application/json",
                RelativePath = $"face/v1.0/detect{additionalArguments}",
                Body = JsonConvert.SerializeObject(new { url }),
                Cost = ServiceCost.FaceApiTransation(returnFaceAttributes == null ? 1 : 2),
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236"
            };
        }

        public static HttpRequest Identify(string personGroupId, IEnumerable<string> faceIds, int maxNumOfCandidatesReturned = 1, float? confidenceThreshold = 0.5f)
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
                Cost = ServiceCost.FaceApiTransation(1),
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395239"
            };
        }

        private static string GenerateDetectRequestArguments(string? returnFaceAttributes, bool? returnFaceLandmarks, string? recognitionModel, string? detectionModel)
        {
            string additionalArguments = $"?recognitionModel={recognitionModel}&detectionModel={detectionModel}";
            if (returnFaceAttributes != null)
            {
                additionalArguments += $"&returnFaceAttributes={returnFaceAttributes}";
            }

            if (returnFaceLandmarks != null)
            {
                additionalArguments += $"&returnFaceLandmarks={returnFaceLandmarks}";
            }

            return additionalArguments;
        }
    }
}
