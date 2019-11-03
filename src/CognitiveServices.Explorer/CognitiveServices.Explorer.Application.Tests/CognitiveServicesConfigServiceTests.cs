using Blazored.LocalStorage;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CognitiveServices.Explorer.Application.Tests
{
    public class CognitiveServicesConfigServiceTests
    {
        [Fact]
        public void ShouldPass()
        {
            true.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldGetDefaultProfile()
        {
            var configService = new CognitiveServicesConfigService(GetMemoryCache(), new Mock<ILocalStorageService>().Object);

            var selectedProfile = await configService.GetSelectedProfileName("FaceApi");

            selectedProfile.Should().Be("default");
        }

        [Fact]
        public async Task ShouldGetSelectedProfile()
        {
            var mockLocalStorage = new Mock<ILocalStorageService>();
            mockLocalStorage
                .Setup(x => x.GetItemAsync<string>("cs-config-profile-FaceApi-selected"))
                .ReturnsAsync("PoC");

            var configService = new CognitiveServicesConfigService(GetMemoryCache(), mockLocalStorage.Object);
            var selectedProfile = await configService.GetSelectedProfileName("FaceApi");

            selectedProfile.Should().Be("PoC");
        }

        [Fact]
        public async Task ShouldGetNoProfile()
        {
            var configService = new CognitiveServicesConfigService(GetMemoryCache(), new Mock<ILocalStorageService>().Object);

            var profile = await configService.GetConfig("FaceApi");

            profile.Should().BeNull();
        }

        [Fact]
        public async Task ShouldGetNonDefaultProfile()
        {
            var mockLocalStorage = new Mock<ILocalStorageService>();
            mockLocalStorage
                .Setup(x => x.GetItemAsync<string>("cs-config-profile-FaceApi-selected"))
                .ReturnsAsync("PoC");
            mockLocalStorage
                .Setup(x => x.GetItemAsync<Dictionary<string, CognitiveServiceConfig>>("cs-config-profile-FaceApi"))
                .ReturnsAsync(new Dictionary<string, CognitiveServiceConfig>
                {
                    { "default", new CognitiveServiceConfig("FaceApi", "default", "def-url", "def-sub-key") },
                    { "PoC", new CognitiveServiceConfig("FaceApi", "PoC", "poc-url", "poc-sub-key") }
                });

            var configService = new CognitiveServicesConfigService(GetMemoryCache(), mockLocalStorage.Object);

            var profile = await configService.GetConfig("FaceApi");

            profile.Should().NotBeNull();
            profile!.ProfileName.Should().Be("PoC");
        }

        [Fact]
        public async Task ShouldFailWhenInvalidArguments()
        {
            var configService = new CognitiveServicesConfigService(GetMemoryCache(), new Mock<ILocalStorageService>().Object);

            await FluentActions.Awaiting(() =>
                configService.SetConfig(new CognitiveServiceConfig
                {
                    ServiceName = "FaceApi",
                    ProfileName = "",
                    Token = "dsfsdf"
                }))
                .Should()
                .ThrowExactlyAsync<ArgumentException>()
                .WithMessage("Base URL is invalid. (Parameter 'BaseUrl')");

            await FluentActions.Awaiting(() =>
                configService.SetConfig(new CognitiveServiceConfig
                {
                    ServiceName = "",
                    ProfileName = "",
                    BaseUrl = "https://test.cognitiveservices.azure.com",
                    Token = "dsfsdf"
                }))
                .Should()
                .ThrowExactlyAsync<ArgumentException>()
                .WithMessage("Service name can't be empty. (Parameter 'ServiceName')");

            await FluentActions.Awaiting(() =>
                configService.SetConfig(new CognitiveServiceConfig
                {
                    ServiceName = "FaceApi",
                    ProfileName = "",
                    BaseUrl = "https://test.cognitiveservices.azure.com",
                    Token = ""
                }))
                .Should()
                .ThrowExactlyAsync<ArgumentException>()
                .WithMessage("Subscription key/token can't be empty. (Parameter 'Token')");
        }

        [Fact]
        public async Task ShouldSaveFromScratch()
        {
            var mockLocalStorage = new Mock<ILocalStorageService>();
            mockLocalStorage
                .Setup(x => x.SetItemAsync("cs-config-profile-FaceApi-selected", "PoC"))
                .Verifiable("Should change selected profile!");
            mockLocalStorage
                .Setup(x =>
                    x.SetItemAsync(
                        "cs-config-profile-FaceApi",
                        It.Is<Dictionary<string, CognitiveServiceConfig>>(
                            d => d.Values.Count == 1 && d.ContainsKey("PoC") )))
                .Verifiable("Should store a dictionary of profiles");

            var configService = new CognitiveServicesConfigService(GetMemoryCache(), mockLocalStorage.Object);
            await configService.SetConfig(new CognitiveServiceConfig
            {
                ServiceName = "FaceApi",
                ProfileName = "PoC",
                BaseUrl = "https://test.cognitiveservices.azure.com",
                Token = "test-token"
            });

            mockLocalStorage.Verify();
        }

        [Fact]
        public async Task ShouldFailValidationOnSelectingProfile()
        {
            var mockLocalStorage = new Mock<ILocalStorageService>();
            mockLocalStorage
                .Setup(x => x.GetItemAsync<string>("cs-config-profile-FaceApi-selected"))
                .ReturnsAsync("PoC");
            mockLocalStorage
                .Setup(x => x.GetItemAsync<Dictionary<string, CognitiveServiceConfig>>("cs-config-profile-FaceApi"))
                .ReturnsAsync(new Dictionary<string, CognitiveServiceConfig>
                {
                    { "default", new CognitiveServiceConfig("FaceApi", "default", "def-url", "def-sub-key") },
                    { "PoC", new CognitiveServiceConfig("FaceApi", "PoC", "poc-url", "poc-sub-key") }
                });
            var configService = new CognitiveServicesConfigService(GetMemoryCache(), mockLocalStorage.Object);

            await FluentActions.Awaiting(() =>
                configService.SetSelectedProfile("FaceApi", ""))
                .Should()
                .ThrowExactlyAsync<ArgumentException>()
                .WithMessage("Profile name can't be empty. (Parameter 'profileName')");

            await FluentActions.Awaiting(() =>
                configService.SetSelectedProfile("FaceApi", "DoesNotExists"))
                .Should()
                .ThrowExactlyAsync<ArgumentException>()
                .WithMessage("Profile DoesNotExists doesn't exists. (Parameter 'profileName')");

            await FluentActions.Awaiting(() =>
                configService.SetSelectedProfile("", "PoC"))
                .Should()
                .ThrowExactlyAsync<ArgumentException>()
                .WithMessage("Service can't be empty. (Parameter 'serviceName')");
        }

        private IMemoryCache GetMemoryCache()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetService<IMemoryCache>();
        }
    }
}
