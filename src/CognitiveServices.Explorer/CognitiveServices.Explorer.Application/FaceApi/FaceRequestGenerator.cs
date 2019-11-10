using CognitiveServices.Explorer.Domain.Face;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CognitiveServices.Explorer.Application.FaceApi
{
    public static class FaceRequestGenerator
    {
        public const string DefaultRecognitionModel = FaceApiConstants.RecognitionModelV01;
        public const string DefaultDetectionModel = FaceApiConstants.DetectModelV01;

        public static HttpRequest Detect(
            byte[] data,
            string? returnFaceAttributes = null,
            bool? returnFaceLandmarks = null,
            string recognitionModel = DefaultRecognitionModel,
            string detectionModel = DefaultDetectionModel)
        {
            var queries = GenerateQueries(returnFaceAttributes, returnFaceLandmarks, recognitionModel, detectionModel);

            return new HttpRequest
            {
                HttpMethod = HttpMethods.Post,
                BinaryContent = data,
                ContentType = "application/octet-stream",
                RelativePath = $"face/v1.0/detect",
                Queries = queries,
                Cost = ServiceCost.FaceApiTransaction(returnFaceAttributes == null ? 1 : 2),
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236"
            };
        }

        public static HttpRequest Detect(
            string url,
            string? returnFaceAttributes = null,
            bool? returnFaceLandmarks = null,
            string recognitionModel = DefaultRecognitionModel,
            string detectionModel = DefaultDetectionModel)
        {
            var queries = GenerateQueries(returnFaceAttributes, returnFaceLandmarks, recognitionModel, detectionModel);

            return new HttpRequest
            {
                HttpMethod = HttpMethods.Post,
                ContentType = "application/json",
                RelativePath = $"face/v1.0/detect",
                Queries = queries,
                Body = JsonConvert.SerializeObject(new { url }),
                Cost = ServiceCost.FaceApiTransaction(returnFaceAttributes == null ? 1 : 2),
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395236"
            };
        }

        public static HttpRequest Identify(string personGroupId, IEnumerable<string> faceIds, int maxNumOfCandidatesReturned = 1, float? confidenceThreshold = 0.5f)
        {
            return new HttpRequest
            {
                HttpMethod = HttpMethods.Post,
                ContentType = "application/json",
                RelativePath = "face/v1.0/identify",
                Body = JsonConvert.SerializeObject(new
                {
                    personGroupId,
                    faceIds,
                    maxNumOfCandidatesReturned,
                    confidenceThreshold
                }),
                Cost = ServiceCost.FaceApiTransaction(1),
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395239"
            };
        }

        private static Dictionary<string, string> GenerateQueries(string? returnFaceAttributes, bool? returnFaceLandmarks, string recognitionModel, string detectionModel)
        {
            var queries = new Dictionary<string, string>
            {
                { "recognitionModel", recognitionModel },
                { "detectionModel", detectionModel }
            };

            if (returnFaceAttributes != null)
            {
                queries.Add("returnFaceAttributes", returnFaceAttributes);
            }

            if (returnFaceLandmarks != null)
            {
                queries.Add("returnFaceLandmarks", returnFaceLandmarks.ToString());
            }

            return queries;
        }
    }
}
