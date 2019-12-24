using Blazored.LocalStorage;
using CognitiveServices.Explorer.Application.Persistence.Profiles;
using CognitiveServices.Explorer.Application.Persistence.Profiles.Migrations;
using CognitiveServices.Explorer.Domain.Profiles;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CognitiveServices.Explorer.Application.Tests.Persistence.Profiles
{
    public class ProfilesRepositoryTests
    {
        private readonly Mock<ILocalStorageService> _localStorageServiceMock = new Mock<ILocalStorageService>();
        private readonly Mock<IProfileMigrations> _migratorMock = new Mock<IProfileMigrations>();

        [Fact]
        public async Task ShouldReturnEmptyProfileList()
        {
            var repo = new ProfilesRepository(_localStorageServiceMock.Object, _migratorMock.Object);

            _migratorMock
                .Setup(x => x.StartMigration(null))
                .ReturnsAsync(new ProfileStorageContainer())
                .Verifiable();

            var profiles = await repo.GetProfiles();

            profiles.Should().BeEmpty();
            _migratorMock.Verify();
        }

        [Fact]
        public async Task ShouldReturnProfiles()
        {
            Guid defaultProfileId = Guid.NewGuid();
            _localStorageServiceMock
                .Setup(x => x.GetItemAsync<ProfileStorageContainer>("Profiles"))
                .ReturnsAsync(new ProfileStorageContainer
                {
                    Profiles = new List<Profile>
                    {
                        new Profile
                        {
                            Id = defaultProfileId,
                            ProfileName = "default",
                            FaceApiConfig = new Domain.Profiles.CognitiveServiceConfig
                            {
                                BaseUrl = "http",
                                ServiceName = "FaceApi",
                                Token = "t1"
                            }
                        }
                    }
                })
                .Verifiable();

            var repo = new ProfilesRepository(_localStorageServiceMock.Object, _migratorMock.Object);

            var profiles = await repo.GetProfiles();

            profiles.Should()
                .SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be(defaultProfileId);
                        first.ProfileName.Should().Be("default");
                        first.FaceApiConfig.Should().NotBeNull();
                        first.FaceApiConfig!.BaseUrl.Should().Be("http");
                        first.FaceApiConfig.ServiceName.Should().Be("FaceApi");
                        first.FaceApiConfig.Token.Should().Be("t1");
                    });

            _migratorMock.Verify(x => x.StartMigration(It.IsAny<ProfileStorageContainer>()), Times.Never);
            _localStorageServiceMock.Verify();
        }
    }
}
