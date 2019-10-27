using CognitiveServices.Explorer.Application.FaceApi;
using Flurl.Http.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CognitiveServices.Explorer.Application.Tests
{
    public class HttpRequestServiceTests
    {
        [Fact]
        public async Task ShouldQueryPersonGroupList()
        {
            using var httpTest = new HttpTest();
            httpTest.RespondWith("{}");

            var request = PersonGroupRequestGenerator.List();
            var httpRequestService = new HttpRequestService();

            var config = new CognitiveServiceConfig("FaceApi", "http://cs-explorer.com", "test-token");
            await httpRequestService.Send(request, config);

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

            var request = new PersonGroupRequestGenerator().Delete("default-group");
            var httpRequestService = new HttpRequestService();

            var config = new CognitiveServiceConfig("FaceApi", "http://cs-explorer.com", "test-token");
            await httpRequestService.Send(request, config);

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

            var request = new PersonGroupRequestGenerator().Update("default-group", "test name");
            var httpRequestService = new HttpRequestService();

            var config = new CognitiveServiceConfig("FaceApi", "http://cs-explorer.com", "test-token");
            await httpRequestService.Send(request, config);

            httpTest
                .ShouldHaveCalled("http://cs-explorer.com/face/v1.0/persongroups/default-group")
                .WithVerb(HttpMethod.Patch)
                .WithHeader("Ocp-Apim-Subscription-Key", "test-token")
                .Times(1);
        }
    }
}
