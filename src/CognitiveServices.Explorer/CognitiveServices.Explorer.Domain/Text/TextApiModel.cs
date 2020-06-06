namespace CognitiveServices.Explorer.Domain.Text
{
    public class TextApiRequest
    {
        public Document[] documents { get; set; }
    }

    public class Document
    {
        public string? id { get; set; }
        public string? language { get; set; }
        public string text { get; set; }
    }

    public class TextApiResponse
    {
        public AnalysedDocument[] documents { get; set; }
        public string[]? errors { get; set; }
        public string? modelVersion { get; set; }
    }

    // This structure is a mixture of 2.1, 3.0 and 3.1 Preview models.
    public class AnalysedDocument
    {
        public string id { get; set; }
        public string language { get; set; }
        public double score { get; set; }
        public string[] keyPhrases { get; set; }
        public Entity[] entities { get; set; }

        // For v3.0+ Text API
        public string sentiment { get; set; }
        public ConfidenceScores confidenceScores { get; set; }
        public Sentence[] sentences { get; set; }
        public string[]? warnings { get; set; }

        // Detect language v2.1
        public DetectedLanguageV21 detectedLanguage { get; set; }

        // Detect language v3.0+
        public DetectedLanguage[] detectedLanguages { get; set; }
    }

    public class DetectedLanguageV21
    {
        public string name { get; set; }
        public string iso6391Name { get; set; }
        public double confidenceScore { get; set; }
    }

    public class DetectedLanguage
    {
        public string name { get; set; }
        public string iso6391Name { get; set; }
        public double score { get; set; }
    }

    public class Entity
    {
        // v2.1
        public string name { get; set; }
        public string wikipediaLanguage { get; set; }
        public string wikipediaId { get; set; }
        public string wikipediaUrl { get; set; }
        public string bingId { get; set; }
        public string type { get; set; }

        // v3.0+
        public string text { get; set; }
        public string category { get; set; }
        public int offset { get; set; }
        public int length { get; set; }
        public double confidenceScore { get; set; }
    }

    public class ConfidenceScores
    {
        public double positive { get; set; }
        public double neutral { get; set; }
        public double negative { get; set; }
    }

    public class Sentence
    {
        public string sentiment { get; set; }
        public ConfidenceScores confidenceScores { get; set; }
        public int offset { get; set; }
        public int length { get; set; }
        public string text { get; set; }
    }
}
