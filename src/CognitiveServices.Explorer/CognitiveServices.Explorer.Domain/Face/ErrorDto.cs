namespace CognitiveServices.Explorer.Domain.Face
{
    public class ErrorDto
    {
        public Error Error { get; set; }
    }

    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
