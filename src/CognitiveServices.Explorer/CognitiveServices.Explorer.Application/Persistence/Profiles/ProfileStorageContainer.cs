using CognitiveServices.Explorer.Domain.Profiles;

namespace CognitiveServices.Explorer.Application.Persistence.Profiles
{
    public class ProfileStorageContainer : StorageContainer<Profile>
    {
        public const int CurrentVersion = 2;

        public ProfileStorageContainer()
        {
            Version = CurrentVersion;
        }

        public string StorageTableName { get; } = "Profiles";
    }
}
