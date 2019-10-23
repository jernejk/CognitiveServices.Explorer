using CognitiveServices.Explorer.Application.FaceApi;
using FluentAssertions;
using Snapshooter.Xunit;
using Xunit;

namespace CognitiveServices.Explorer.Application.Tests.FaceApi
{
    public class PersonGroupCurlGeneratorTests
    {
        [Fact]
        public void ShouldGenerateCreate()
        {
            new PersonGroupCurlGenerator()
                .Create("default", "Test group")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateCreateWithUserData()
        {
            new PersonGroupCurlGenerator()
                .Create("default", "Test group 2", "imageUrl: image")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateUpdate()
        {
            new PersonGroupCurlGenerator()
                .Update("default", "Test group 2", "imageUrl: image")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateList()
        {
            new PersonGroupCurlGenerator()
                .List()
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateDelete()
        {
            new PersonGroupCurlGenerator()
                .Delete("default")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateTrain()
        {
            new PersonGroupCurlGenerator()
                .Train("default")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateCheckTraining()
        {
            new PersonGroupCurlGenerator()
                .CheckTraining("default")
                .Should()
                .MatchSnapshot();
        }
    }
}
