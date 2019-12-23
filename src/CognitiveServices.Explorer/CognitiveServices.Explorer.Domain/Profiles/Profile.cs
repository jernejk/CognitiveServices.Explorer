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

        public CognitiveServiceConfig? FaceApiConfig { get; set; }
        public CognitiveServiceConfig? TextApiConfig { get; set; }
        public CognitiveServiceConfig? SpeechApiConfig { get; set; }
    }
}
