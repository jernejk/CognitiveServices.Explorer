using CognitiveServices.Explorer.Domain.Text;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;

namespace CognitiveServices.Explorer.Application.Text
{
    public static class TextRequestGenerator
    {
        public const string LegacyVersion = "2.1";
        public const string StableVersion = "3.0";
        public const string PreviewVersion = "3.1-preview.1";
        public const string DefaultLanguage = "en";

        public static HttpRequest DetectLanguage(string text, string language = DefaultLanguage, string version = StableVersion)
        {
            string docUrl = version switch
            {
                LegacyVersion => "https://westcentralus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v2-1/operations/56f30ceeeda5650db055a3c7",
                StableVersion => "https://westus2.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v3-0/operations/Languages",
                PreviewVersion => "https://westcentralus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v3-1-preview-1/operations/Languages",
                _ => "Unknown version"
            };

            return MakeSingleRequest($"v{version}/languages", text, language, docUrl);
        }

        public static HttpRequest Sentiment(string text, string language = DefaultLanguage, string version = StableVersion)
        {
            string docUrl = version switch
            {
                LegacyVersion => "https://westcentralus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v2-1/operations/56f30ceeeda5650db055a3c9",
                StableVersion => "https://westus2.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v3-0/operations/Sentiment",
                PreviewVersion => "https://westcentralus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v3-1-preview-1/operations/Sentiment",
                _ => "Unknown version"
            };

            return MakeSingleRequest($"v{version}/sentiment", text, language, docUrl);
        }

        public static HttpRequest Entities(string text, string language = DefaultLanguage, string version = StableVersion)
        {
            string docUrl = version switch
            {
                LegacyVersion => "https://westcentralus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v2-1/operations/5ac4251d5b4ccd1554da7634",
                StableVersion => "https://westus2.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v3-0/operations/EntitiesRecognitionGeneral",
                PreviewVersion => "https://westcentralus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v3-1-preview-1/operations/EntitiesRecognitionGeneral",
                _ => "Unknown version"
            };

            string url = version == LegacyVersion ?
                "v2.1/entities" :
                $"v{version}/entities/recognition/general";
            return MakeSingleRequest(url, text, language, docUrl);
        }

        public static HttpRequest KeyPhrases(string text, string language = DefaultLanguage, string version = StableVersion)
        {
            string docUrl = version switch
            {
                LegacyVersion => "https://westcentralus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v2-1/operations/56f30ceeeda5650db055a3c6",
                StableVersion => "https://westus2.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v3-0/operations/KeyPhrases",
                PreviewVersion => "https://westcentralus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v3-1-preview-1/operations/KeyPhrases",
                _ => "Unknown version"
            };

            return MakeSingleRequest($"v{version}/keyPhrases", text, language, docUrl);
        }

        public static HttpRequest EntityLinking(string text, string language = DefaultLanguage, string version = StableVersion)
        {
            string docUrl = version switch
            {
                LegacyVersion => "Not supported",
                StableVersion => "https://westus2.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v3-0/operations/EntitiesLinking",
                PreviewVersion => "https://westcentralus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v3-1-preview-1/operations/EntitiesLinking",
                _ => "Unknown version"
            };

            return MakeSingleRequest($"v{version}/entities/linking", text, language, docUrl);
        }

        public static HttpRequest EntityRecognitionPii(string text, string language = DefaultLanguage, string version = PreviewVersion)
        {
            string docUrl = version switch
            {
                LegacyVersion => "Not supported",
                StableVersion => "Not supported",
                PreviewVersion => "https://westcentralus.dev.cognitive.microsoft.com/docs/services/TextAnalytics-v3-1-preview-1/operations/EntitiesRecognitionPii",
                _ => "Unknown version"
            };

            return MakeSingleRequest($"v{version}/entities/recognition/pii", text, language, docUrl);
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
                            // ID can be user defined but for sake of simplicity we are generating it.
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
