using CognitiveServices.Explorer.Application.Commands;
using CognitiveServices.Explorer.Application.FaceApi;
using CognitiveServices.Explorer.Domain.Profiles;
using Flurl.Http.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CognitiveServices.Explorer.Application.Tests
{
    public class ExecuteCognitiveServicesCommandTests
    {
        private readonly ExecuteCognitiveServicesCommand.Handler _handler;

        public ExecuteCognitiveServicesCommandTests()
        {
            _handler = new ExecuteCognitiveServicesCommand.Handler();
        }

        [Fact]
        public async Task ShouldPass()
        {
            using var httpTest = new HttpTest();
            httpTest.RespondWith("{}");

            var request = PersonGroupRequestGenerator.List();
            var config = new CognitiveServiceConfig("FaceApi", "http://cs-explorer.com", "test-token");
            await _handler.Handle(new ExecuteCognitiveServicesCommand(request, config), default);

            httpTest
                .ShouldHaveCalled("http://cs-explorer.com/face/v1.0/persongroups?returnRecognitionModel=true")
                .WithVerb(HttpMethod.Get)
                .WithHeader("Ocp-Apim-Subscription-Key", "test-token")
                .Times(1);
        }

        [Fact]
        public async Task ShouldDeletePersonGroup()
        {
            using var httpTest = new HttpTest();
            httpTest.RespondWith("{}");

            var request = PersonGroupRequestGenerator.Delete("default-group");
            var config = new CognitiveServiceConfig("FaceApi", "http://cs-explorer.com", "test-token");
            await _handler.Handle(new ExecuteCognitiveServicesCommand(request, config), default);

            httpTest
                .ShouldHaveCalled("http://cs-explorer.com/face/v1.0/persongroups/default-group")
                .WithVerb(HttpMethod.Delete)
                .WithHeader("Ocp-Apim-Subscription-Key", "test-token")
                .Times(1);
        }

        [Fact]
        public async Task ShouldUpdatePersonGroup()
        {
            using var httpTest = new HttpTest();
            httpTest.RespondWith("{}");

            var request = PersonGroupRequestGenerator.Update("default-group", "test name");
            var config = new CognitiveServiceConfig("FaceApi", "http://cs-explorer.com", "test-token");
            await _handler.Handle(new ExecuteCognitiveServicesCommand(request, config), default);

            httpTest
                .ShouldHaveCalled("http://cs-explorer.com/face/v1.0/persongroups/default-group")
                .WithVerb(HttpMethod.Patch)
                .WithHeader("Ocp-Apim-Subscription-Key", "test-token")
                .Times(1);
        }
    }
}
