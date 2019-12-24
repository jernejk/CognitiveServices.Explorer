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

        public CognitiveServiceConfig? FaceApiConfig { get; set; } = new CognitiveServiceConfig();
        public CognitiveServiceConfig? TextApiConfig { get; set; } = new CognitiveServiceConfig();
        public CognitiveServiceConfig? SpeechApiConfig { get; set; } = new CognitiveServiceConfig();

        public void Map(Profile profile)
        {
            Id = profile.Id;
            ProfileName = profile.ProfileName;
            IsSelected = profile.IsSelected;
            FaceApiConfig = profile.FaceApiConfig;
            TextApiConfig = profile.TextApiConfig;
            SpeechApiConfig = profile.SpeechApiConfig;
        }
    }
}
