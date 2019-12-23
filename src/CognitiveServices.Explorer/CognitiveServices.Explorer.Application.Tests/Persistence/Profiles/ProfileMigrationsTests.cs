using Blazored.LocalStorage;
using CognitiveServices.Explorer.Application.Persistence.Profiles;
using CognitiveServices.Explorer.Application.Persistence.Profiles.Migrations;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CognitiveServices.Explorer.Application.Tests.Persistence.Profiles
{
    public class ProfileMigrationsTests
    {
        private readonly Mock<ILocalStorageService> _localStorageServiceMock = new Mock<ILocalStorageService>();

        [Fact]
        public async Task ShouldMigrateFromScratch()
        {
            var migrator = new ProfileMigrations(_localStorageServiceMock.Object);

            var container = await migrator.StartMigration(null);

            container.Should().NotBeNull();
            container.Version.Should().Be(2);
            container.Profiles.Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldNotMigrateIfAlreadyMigrated()
        {
            var migrator = new ProfileMigrations(_localStorageServiceMock.Object);

            var container = await migrator.StartMigration(new ProfileStorageContainer
            {
                Version = 2
            });

            container.Should().NotBeNull();
            container.Version.Should().Be(2);
            container.Profiles.Should().BeEmpty();

            // Should not use local storage.
            _localStorageServiceMock.Invocations.Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldMigrateFromExistingConfigPartial()
        {
            var faceApiConfig = new Dictionary<string, CognitiveServiceConfig>
            {
                { "MyCompany", new CognitiveServiceConfig("FaceApi", "MyCompany", "http://mycompany-cservice.com", "token1") },
                { "Contoso", new CognitiveServiceConfig("FaceApi", "Contoso", "http://contoso-cservice.com", "token2") }
            };

            _localStorageServiceMock
                .Setup(x => x.GetItemAsync<Dictionary<string, CognitiveServiceConfig>>("cs-config-profile-FaceApi"))
                .ReturnsAsync(faceApiConfig)
                .Verifiable();

            _localStorageServiceMock
                .Setup(x => x.SetItemAsync("Profiles", It.IsAny<object>()))
                .Verifiable();

            var migrator = new ProfileMigrations(_localStorageServiceMock.Object);

            var container = await migrator.StartMigration(null);

            container.Should().NotBeNull();
            container.Version.Should().Be(2);
            container.Profiles.Should()
                .SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().NotBe(Guid.Empty);
                        first.ProfileName.Should().Be("MyCompany");
                        first.IsSelected.Should().BeTrue();
                        first.FaceApiConfig.Should().NotBeNull();
                        first.FaceApiConfig.ServiceName.Should().Be("FaceApi");
                        first.FaceApiConfig.BaseUrl.Should().Be("http://mycompany-cservice.com");
                        first.FaceApiConfig.Token.Should().Be("token1");
                    },
                    second =>
                    {
                        second.Id.Should().NotBe(Guid.Empty);
                        second.ProfileName.Should().Be("Contoso");
                        second.IsSelected.Should().BeFalse();
                        second.FaceApiConfig.Should().NotBeNull();
                        second.FaceApiConfig.ServiceName.Should().Be("FaceApi");
                        second.FaceApiConfig.BaseUrl.Should().Be("http://contoso-cservice.com");
                        second.FaceApiConfig.Token.Should().Be("token2");
                    });

            _localStorageServiceMock.Invocations.Should().HaveCount(5);
            _localStorageServiceMock.Verify();
        }

        [Fact]
        public async Task ShouldMigrateFromExistingConfigFull()
        {
            var faceApiConfig = new Dictionary<string, CognitiveServiceConfig>
            {
                { "MyCompany", new CognitiveServiceConfig("FaceApi", "MyCompany", "http://mycompany-cservice.com", "token1") },
                { "Contoso", new CognitiveServiceConfig("FaceApi", "Contoso", "http://contoso-cservice.com", "token2") },
                { "default", new CognitiveServiceConfig("FaceApi", "default", "http://face-default-cservice.com", "token9") }
            };

            var textApiConfig = new Dictionary<string, CognitiveServiceConfig>
            {
                { "default", new CognitiveServiceConfig("TextApi", "default", "http://default-text-cservice.com", "token3") }
            };

            var speechApiConfig = new Dictionary<string, CognitiveServiceConfig>
            {
                { "default", new CognitiveServiceConfig("SpeechApi", "default", "http://default-speech-cservice.com", "token3") }
            };

            _localStorageServiceMock
                .Setup(x => x.GetItemAsync<Dictionary<string, CognitiveServiceConfig>>("cs-config-profile-FaceApi"))
                .ReturnsAsync(faceApiConfig)
                .Verifiable();

            _localStorageServiceMock
                .Setup(x => x.GetItemAsync<string>("cs-config-profile-FaceApi-selected"))
                .ReturnsAsync("Contoso")
                .Verifiable();

            _localStorageServiceMock
                .Setup(x => x.GetItemAsync<Dictionary<string, CognitiveServiceConfig>>("cs-config-profile-TextApi"))
                .ReturnsAsync(textApiConfig)
                .Verifiable();

            _localStorageServiceMock
                .Setup(x => x.GetItemAsync<Dictionary<string, CognitiveServiceConfig>>("cs-config-profile-SpeechApi"))
                .ReturnsAsync(speechApiConfig)
                .Verifiable();

            _localStorageServiceMock
                .Setup(x => x.SetItemAsync("Profiles", It.IsAny<object>()))
                .Verifiable();

            var migrator = new ProfileMigrations(_localStorageServiceMock.Object);

            var container = await migrator.StartMigration(null);

            container.Should().NotBeNull();
            container.Version.Should().Be(2);
            container.Profiles.Should()
                .SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().NotBe(Guid.Empty);
                        first.ProfileName.Should().Be("MyCompany");
                        first.IsSelected.Should().BeFalse();
                        first.FaceApiConfig.Should().NotBeNull();
                        first.FaceApiConfig!.ServiceName.Should().Be("FaceApi");
                        first.FaceApiConfig.BaseUrl.Should().Be("http://mycompany-cservice.com");
                        first.FaceApiConfig.Token.Should().Be("token1");
                        first.TextApiConfig.Should().BeNull();
                        first.SpeechApiConfig.Should().BeNull();
                    },
                    second =>
                    {
                        second.Id.Should().NotBe(Guid.Empty);
                        second.ProfileName.Should().Be("Contoso");
                        second.IsSelected.Should().BeTrue();
                        second.FaceApiConfig.Should().NotBeNull();
                        second.FaceApiConfig!.ServiceName.Should().Be("FaceApi");
                        second.FaceApiConfig.BaseUrl.Should().Be("http://contoso-cservice.com");
                        second.FaceApiConfig.Token.Should().Be("token2");
                        second.TextApiConfig.Should().BeNull();
                        second.SpeechApiConfig.Should().BeNull();
                    },
                    third =>
                    {
                        third.Id.Should().NotBe(Guid.Empty);
                        third.ProfileName.Should().Be("default");
                        third.IsSelected.Should().BeFalse();

                        third.FaceApiConfig.Should().NotBeNull();
                        third.FaceApiConfig!.ServiceName.Should().Be("FaceApi");
                        third.FaceApiConfig.BaseUrl.Should().Be("http://face-default-cservice.com");
                        third.FaceApiConfig.Token.Should().Be("token9");

                        third.TextApiConfig.Should().NotBeNull();
                        third.TextApiConfig!.ServiceName.Should().Be("TextApi");
                        third.TextApiConfig.BaseUrl.Should().Be("http://default-text-cservice.com");
                        third.TextApiConfig.Token.Should().Be("token3");

                        third.SpeechApiConfig.Should().NotBeNull();
                        third.SpeechApiConfig!.ServiceName.Should().Be("SpeechApi");
                        third.SpeechApiConfig.BaseUrl.Should().Be("http://default-speech-cservice.com");
                        third.SpeechApiConfig.Token.Should().Be("token3");
                    });

            _localStorageServiceMock.Invocations.Should().HaveCount(5);
            _localStorageServiceMock.Verify();
        }
    }
}
