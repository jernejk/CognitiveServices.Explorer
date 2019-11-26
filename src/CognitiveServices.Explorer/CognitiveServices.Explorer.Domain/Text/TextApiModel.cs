namespace CognitiveServices.Explorer.Domain.Text
{
    public class TextApiRequest
    {
        public Document[] documents { get; set; }
    }

    public class Document
    {
        public string id { get; set; }
        public string language { get; set; }
        public string text { get; set; }
    }

    public class TextApiResponse
    {
        public AnalysedDocument[] documents { get; set; }
    }

    public class AnalysedDocument
    {
        public string id { get; set; }
        public string language { get; set; }
        public double score { get; set; }
        public string[] keyPhrases { get; set; }
        public Entity[] entities { get; set; }

        // For v3 preview of text API
        public string sentiment { get; set; }
        public DocumentScore documentScores { get; set; }
    }

    public class Entity
    {
        public string name { get; set; }
        public string type { get; set; }
        public string wikipediaUrl { get; set; }
    }

    public class DocumentScore
    {
        public double positive { get; set; }
        public double neutral { get; set; }
        public double negative { get; set; }
    }
}
