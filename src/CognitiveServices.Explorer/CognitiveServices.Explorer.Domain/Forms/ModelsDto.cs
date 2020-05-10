namespace CognitiveServices.Explorer.Domain.Forms
{

    public class GetModelsDto
    {
        public Model[] modelList { get; set; }
        public Error error { get; set; }
    }

    public class Model
    {
        public string modelId { get; set; }
        public string status { get; set; }
        public string createdDateTime { get; set; }
        public string lastUpdatedDateTime { get; set; }
    }

    public class Error
    {
        public string code { get; set; }
        public Innererror innerError { get; set; }
        public string message { get; set; }
    }

    public class Innererror
    {
        public string requestId { get; set; }
    }
}
