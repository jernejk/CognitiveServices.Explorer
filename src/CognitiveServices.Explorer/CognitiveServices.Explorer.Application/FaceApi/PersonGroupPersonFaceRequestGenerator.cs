using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CognitiveServices.Explorer.Application.FaceApi
{
    public static class PersonGroupPersonFaceRequestGenerator
    {
        public static HttpRequest Add(string groupId, string userId, string imageUrl, string? detectionModel = null, string? userData = null, string? targetFace = null)
        {
            return new HttpRequest
            {
                HttpMethod = HttpMethods.Post,
                RelativePath = $"face/v1.0/persongroups/{groupId}/persons/{userId}/persistedFaces",
                Queries = GenerateQueries(detectionModel, userData, targetFace),
                Body = JsonConvert.SerializeObject(new { url = imageUrl }),
                Cost = ServiceCost.FaceApiTransaction(1),
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f3039523b"
            };
        }

        public static HttpRequest Add(string groupId, string userId, byte[] data, string? detectionModel = null, string? userData = null, string? targetFace = null)
        {
            return new HttpRequest
            {
                HttpMethod = HttpMethods.Post,
                RelativePath = $"face/v1.0/persongroups/{groupId}/persons/{userId}/persistedFaces",
                Queries = GenerateQueries(detectionModel, userData, targetFace),
                BinaryContent = data,
                ContentType = "application/octet-stream",
                Cost = ServiceCost.FaceApiTransaction(1),
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f3039523f"
            };
        }

        public static HttpRequest Get(string groupId, string userId, string faceId)
        {
            return new HttpRequest
            {
                HttpMethod = HttpMethods.Get,
                RelativePath = $"face/v1.0/persongroups/{groupId}/persons/{userId}/persistedFaces/{faceId}",
                Cost = ServiceCost.FaceApiTransaction(1),
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395243"
            };
        }

        public static HttpRequest Update(string groupId, string userId, string faceId, string userData)
        {
            return new HttpRequest
            {
                HttpMethod = HttpMethods.Patch,
                RelativePath = $"face/v1.0/persongroups/{groupId}/persons/{userId}/persistedFaces/{faceId}",
                Body = JsonConvert.SerializeObject(new { userData }),
                Cost = ServiceCost.FaceApiTransaction(1),
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f30395243"
            };
        }

        public static HttpRequest Delete(string groupId, string userId, string faceId)
        {
            return new HttpRequest
            {
                HttpMethod = HttpMethods.Delete,
                RelativePath = $"face/v1.0/persongroups/{groupId}/persons/{userId}/persistedFaces/{faceId}",
                Cost = ServiceCost.FaceApiTransaction(1),
                CognitiveServiceDoc = "https://westus.dev.cognitive.microsoft.com/docs/services/563879b61984550e40cbbe8d/operations/563879b61984550f3039523e"
            };
        }

        private static Dictionary<string, string> GenerateQueries(string? detectionModel, string? userData, string? targetFace)
        {
            return new Dictionary<string, string>
            {
                { "detectionModel", detectionModel ?? string.Empty },
                { "returnFaceAttributes", userData ?? string.Empty },
                { "returnFaceLandmarks", targetFace ?? string.Empty }
            };
        }
    }
}
