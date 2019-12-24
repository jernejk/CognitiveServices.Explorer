using CognitiveServices.Explorer.Application.Persistence.Profiles;
using CognitiveServices.Explorer.Application.Profiles.Commands;
using CognitiveServices.Explorer.Domain.Profiles;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CognitiveServices.Explorer.Application.Tests.Profiles
{
    public class SaveProfileCommandHandlerTests
    {
        private readonly Mock<IProfilesRepository> _profilesRepoMock = new Mock<IProfilesRepository>();

        [Fact]
        public async Task ShouldCreateProfile()
        {
            Guid profileId = Guid.NewGuid();
            _profilesRepoMock
                .Setup(x => x.GetProfiles())
                .ReturnsAsync(new List<Profile>())
                .Verifiable();

            var profile = new Profile
            {
                Id = profileId,
                ProfileName = "test2",
                FaceApiConfig = new CognitiveServiceConfig
                {
                    BaseUrl = "http://url",
                    Token = "token"
                }
            };

            var handler = new SaveProfileCommand.Handler(_profilesRepoMock.Object);

            await handler.Handle(new SaveProfileCommand(profile), default);

            _profilesRepoMock.Verify();
            _profilesRepoMock
                .Verify(x => x.SaveProfiles(
                    It.Is<List<Profile>>(list => list.Count == 1 && list[0].Id != Guid.Empty)), Times.Once());
        }

        [Fact]
        public async Task ShouldUpdateProfile()
        {
            Guid profileId = Guid.NewGuid();
            _profilesRepoMock
                .Setup(x => x.GetProfiles())
                .ReturnsAsync(new List<Profile>
                {
                    new Profile("test1")
                    {
                        IsSelected = true
                    },
                    new Profile("test2")
                    {
                        Id = profileId
                    },
                    new Profile("test3")
                })
                .Verifiable();

            var profile = new Profile
            {
                Id = profileId,
                ProfileName = "test2",
                FaceApiConfig = new CognitiveServiceConfig
                {
                    BaseUrl = "http://url",
                    Token = "token"
                }
            };

            var handler = new SaveProfileCommand.Handler(_profilesRepoMock.Object);
           
            await handler.Handle(new SaveProfileCommand(profile), default);

            _profilesRepoMock.Verify();
            _profilesRepoMock
                .Verify(x => x.SaveProfiles(
                    It.Is<List<Profile>>(list => VerifyProfileIsUpdated(list, profile))), Times.Once());
        }

        private bool VerifyProfileIsUpdated(List<Profile> profiles, Profile updatedProfile)
        {
            var selectedProfilesCount = profiles.Count(p => p.IsSelected);
            var profile = profiles.FirstOrDefault(p => p.Id == updatedProfile.Id);

            return
                selectedProfilesCount == 1 &&
                !profile.IsSelected &&
                profile.FaceApiConfig!.BaseUrl == "http://url" &&
                profile.FaceApiConfig.Token == "token";
        }
    }
}
