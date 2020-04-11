namespace CognitiveServices.Explorer.Web
{
    /// <summary>
    /// Application configuration meant to be constants or be set as part of build pipeline.
    /// </summary>
    public class AppConfiguration
    {
        public bool IsDev { get; set; }
        public string Environment { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string TitleImg { get; set; } = null!;
    }
}
