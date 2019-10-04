namespace CognitiveServices.Explorer.Domain.Face
{
    public class UserDataWithImageUrl
    {
        public string ImageUrl { get; set; }
        public string PictureUrl { get; set; }

        public string GetImageUrl() => ImageUrl ?? PictureUrl;
    }
}
