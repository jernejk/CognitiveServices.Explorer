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
    public class DeleteProfileCommandHandlerTests
    {
        private readonly Mock<IProfilesRepository> _profilesRepoMock = new Mock<IProfilesRepository>();

        [Fact]
        public async Task ShouldDeleteProfile()
        {
            Guid profileId = Guid.NewGuid();
            _profilesRepoMock
                .Setup(x => x.GetProfiles())
                .ReturnsAsync(new List<Profile>
                {
                    new Profile("test1"),
                    new Profile("test2")
                    {
                        Id = profileId,
                        IsSelected = true
                    },
                    new Profile("test3")
                })
                .Verifiable();

            var handler = new DeleteProfileCommand.Handler(_profilesRepoMock.Object);

            await handler.Handle(new DeleteProfileCommand(profileId), default);

            _profilesRepoMock.Verify();
            _profilesRepoMock
                .Verify(x => x.SaveProfiles(
                    It.Is<List<Profile>>(list => VerifyProfileIsDeleted(list, profileId))), Times.Once());
        }

        private bool VerifyProfileIsDeleted(List<Profile> profiles, Guid profileId)
        {
            // Verify that selected profile has changed to the first one on the list.
            var selectedProfilesCount = profiles.Count(p => p.IsSelected);
            var selectedProfile = profiles.FirstOrDefault(p => p.IsSelected);
            var profile = profiles.FirstOrDefault(p => p.Id == profileId);

            return
                selectedProfilesCount == 1 &&
                selectedProfile?.ProfileName == "test1" &&
                profile == null;
        }
    }
}
