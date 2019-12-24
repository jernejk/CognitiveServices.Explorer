using CognitiveServices.Explorer.Application.Persistence.Profiles;
using CognitiveServices.Explorer.Application.Profiles.Queries;
using CognitiveServices.Explorer.Domain.Profiles;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CognitiveServices.Explorer.Application.Tests.Profiles
{
    public class GetAllProfilesQueryHandlerTests
    {
        private readonly Mock<IProfilesRepository> _profilesRepoMock = new Mock<IProfilesRepository>();

        [Fact]
        public async Task ShouldGetNoProfile()
        {
            _profilesRepoMock
                .Setup(x => x.GetProfiles())
                .ReturnsAsync(new List<Profile>())
                .Verifiable();

            var handler = new GetCurrentProfileQueryHandler(_profilesRepoMock.Object);

            var profile = await handler.Handle(new GetCurrentProfileQuery());

            profile.Should().BeNull();
            _profilesRepoMock.Verify();
        }


        [Fact]
        public async Task ShouldGetSelectedProfile()
        {
            _profilesRepoMock
                .Setup(x => x.GetProfiles())
                .ReturnsAsync(new List<Profile>
                {
                    new Profile("test1"),
                    new Profile("test2")
                    {
                        IsSelected = true
                    },
                    new Profile("test3")
                })
                .Verifiable();

            var handler = new GetCurrentProfileQueryHandler(_profilesRepoMock.Object);

            var profile = await handler.Handle(new GetCurrentProfileQuery());

            profile.Should().NotBeNull();
            profile.ProfileName.Should().Be("test2");
            _profilesRepoMock.Verify();
        }
    }
}
