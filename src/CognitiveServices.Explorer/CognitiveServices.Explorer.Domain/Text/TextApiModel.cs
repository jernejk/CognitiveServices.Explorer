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
    }

    public class Entity
    {
        public string name { get; set; }
        public string type { get; set; }
        public string wikipediaUrl { get; set; }
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
