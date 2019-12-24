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
    public class SelectProfileCommandHandlerTests
    {
        private readonly Mock<IProfilesRepository> _profilesRepoMock = new Mock<IProfilesRepository>();

        [Fact]
        public async Task ShouldSelectProfile()
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

            var handler = new SelectProfileCommandHandler(_profilesRepoMock.Object);

            await handler.Handle(new SelectProfileCommand(profileId));

            _profilesRepoMock.Verify();

            _profilesRepoMock
                .Verify(x => x.SaveProfiles(
                    It.Is<List<Profile>>(list => VerifyProfileIsSelected(list, profileId))), Times.Once());
        }

        private bool VerifyProfileIsSelected(List<Profile> profiles, Guid selectedProfileId)
        {
            var selectedProfilesCount = profiles.Count(p => p.IsSelected);
            var selectedProfile = profiles.FirstOrDefault(p => p.Id == selectedProfileId);

            return selectedProfilesCount == 1 && selectedProfile?.IsSelected == true;
        }
    }
}
