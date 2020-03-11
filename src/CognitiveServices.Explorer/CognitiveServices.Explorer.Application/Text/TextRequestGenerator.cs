using CognitiveServices.Explorer.Domain.Text;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;

namespace CognitiveServices.Explorer.Application.Text
{
    public static class TextRequestGenerator
    {
        public const string StableVersion = "2.1";
        public const string PreviewVersion = "3.0-preview.1";
        public const string DefaultLanguage = "en";

        public static HttpRequest DetectLanguage(string text, string language = DefaultLanguage, string version = StableVersion)
        {
            string docUrl = version == StableVersion ?
                "https://westcentralus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v2-1/operations/56f30ceeeda5650db055a3c7" :
                "https://westus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v3-0-Preview-1/operations/Languages";
            return MakeSingleRequest($"v{version}/languages", text, language, docUrl);
        }

        public static HttpRequest Sentiment(string text, string language = DefaultLanguage, string version = StableVersion)
        {
            string docUrl = version == StableVersion ?
                "https://westcentralus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v2-1/operations/56f30ceeeda5650db055a3c9" :
                "https://westus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v3-0-Preview-1/operations/Sentiment";
            return MakeSingleRequest($"v{version}/sentiment", text, language, docUrl);
        }

        public static HttpRequest Entities(string text, string language = DefaultLanguage, string version = StableVersion)
        {
            string url = version == StableVersion ?
                "v2.1/entities" :
                "v3.0-preview.1/entities/recognition/general";
            string docUrl = version == StableVersion ?
                "https://westcentralus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v2-1/operations/5ac4251d5b4ccd1554da7634" :
                "https://westus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v3-0-Preview-1/operations/EntitiesRecognitionGeneral";
            return MakeSingleRequest(url, text, language, docUrl);
        }

        public static HttpRequest KeyPhrases(string text, string language = DefaultLanguage, string version = StableVersion)
        {
            string url = version == StableVersion ?
                "v2.1/entities" :
                "v3.0-preview.1/entities/recognition/general";
            string docUrl = version == StableVersion ?
                "https://westcentralus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v2-1/operations/56f30ceeeda5650db055a3c6" :
                "https://westus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v3-0-Preview-1/operations/KeyPhrases";
            return MakeSingleRequest(url, text, language, docUrl);
        }

        public static HttpRequest EntityLinking(string text, string language = DefaultLanguage)
        {
            string url = "v3.0-preview.1/entities/linking";
            string docUrl = "https://westus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v3-0-Preview-1/operations/EntitiesLinking";
            return MakeSingleRequest(url, text, language, docUrl);
        }

        public static HttpRequest EntityRecognitionPii(string text, string language = DefaultLanguage)
        {
            string url = "v3.0-preview.1/entities/recognition/pii";
            string docUrl = "https://westus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v3-0-Preview-1/operations/EntitiesRecognitionPii";
            return MakeSingleRequest(url, text, language, docUrl);
        }

        private static HttpRequest MakeSingleRequest(string url, string text, string language, string docUrl)
        {
            return new HttpRequest
            {
                HttpMethod = HttpMethods.Post,
                ContentType = "application/json",
                RelativePath = $"text/analytics/{url}",
                Body = JsonSerializer.Serialize(new TextApiRequest
                {
                    documents = new Document[]
                    {
                        new Document
                        {
                            id = Guid.NewGuid().ToString(),
                            text = text,
                            language = language
                        }
                    }
                }),
                Cost = ServiceCost.TextApiTransaction(1),
                CognitiveServiceDoc = docUrl
            };
        }
    }
}
