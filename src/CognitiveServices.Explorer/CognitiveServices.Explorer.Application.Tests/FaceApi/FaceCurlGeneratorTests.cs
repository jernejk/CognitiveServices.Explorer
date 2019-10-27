using CognitiveServices.Explorer.Application.FaceApi;
using FluentAssertions;
using Snapshooter.Xunit;
using Xunit;

namespace CognitiveServices.Explorer.Application.Tests.FaceApi
{
    public class FaceCurlGeneratorTests
    {
        [Fact]
        public void ShouldGenerateDetectBinary()
        {
            FaceRequestGenerator
                .Detect(new byte[1] { 5 })
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateDetectUrl()
        {
            FaceRequestGenerator
                .Detect("http://test.url")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateIdentify()
        {
            string[] faces = { "12234324", "34534253425" };
            FaceRequestGenerator
                .Identify("default", faces)
                .Should()
                .MatchSnapshot();
        }
    }
}
