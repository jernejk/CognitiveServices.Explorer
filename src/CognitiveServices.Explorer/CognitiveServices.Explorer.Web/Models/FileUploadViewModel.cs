namespace CognitiveServices.Explorer.Web.Models
{
    public record FileUploadViewModel
    {
        public string FileName { get; init; }
        public string Type { get; init; }
        public long Size { get; init; }
        public byte[] Data { get; init; }
    }
}
