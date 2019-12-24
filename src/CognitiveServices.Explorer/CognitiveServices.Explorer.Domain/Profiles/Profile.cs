using System;

namespace CognitiveServices.Explorer.Domain.Profiles
{
    public class Profile
    {
        public Profile()
        {
        }

        public Profile(string profileName)
        {
            Id = Guid.NewGuid();
            ProfileName = profileName;
        }

        public Guid Id { get; set; }
        public string ProfileName { get; set; } = null!;
        public bool IsSelected { get; set; }

        public CognitiveServiceConfig? FaceApiConfig { get; set; } = new CognitiveServiceConfig("FaceApi");
        public CognitiveServiceConfig? TextApiConfig { get; set; } = new CognitiveServiceConfig("TextApi");
        public CognitiveServiceConfig? SpeechApiConfig { get; set; } = new CognitiveServiceConfig("SpeechApi");

        public void Map(Profile profile)
        {
            Id = profile.Id;
            ProfileName = profile.ProfileName;
            IsSelected = profile.IsSelected;
            FaceApiConfig = profile.FaceApiConfig ?? new CognitiveServiceConfig("FaceApi");
            TextApiConfig = profile.TextApiConfig ?? new CognitiveServiceConfig("TextApi");
            SpeechApiConfig = profile.SpeechApiConfig?? new CognitiveServiceConfig("SpeechApi");
        }
    }
}
