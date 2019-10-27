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
            new PersonGroupRequestGenerator()
                .Create("default", "Test group")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateCreateWithUserData()
        {
            new PersonGroupRequestGenerator()
                .Create("default", "Test group 2", "imageUrl: image")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateUpdate()
        {
            new PersonGroupRequestGenerator()
                .Update("default", "Test group 2", "imageUrl: image")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateList()
        {
            new PersonGroupRequestGenerator()
                .List()
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateDelete()
        {
            new PersonGroupRequestGenerator()
                .Delete("default")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateTrain()
        {
            new PersonGroupRequestGenerator()
                .Train("default")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateCheckTraining()
        {
            new PersonGroupRequestGenerator()
                .CheckTraining("default")
                .Should()
                .MatchSnapshot();
        }
    }
}
