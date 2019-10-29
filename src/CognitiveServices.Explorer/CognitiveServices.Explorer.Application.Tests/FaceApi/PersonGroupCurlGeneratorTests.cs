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
            PersonGroupRequestGenerator
                .Create("default", "Test group")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateCreateWithUserData()
        {
            PersonGroupRequestGenerator
                .Create("default", "Test group 2", "imageUrl: image")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateUpdate()
        {
            PersonGroupRequestGenerator
                .Update("default", "Test group 2", "imageUrl: image")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateList()
        {
            PersonGroupRequestGenerator
                .List()
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateDelete()
        {
            PersonGroupRequestGenerator
                .Delete("default")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateTrain()
        {
            PersonGroupRequestGenerator
                .Train("default")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateCheckTraining()
        {
            PersonGroupRequestGenerator
                .CheckTraining("default")
                .Should()
                .MatchSnapshot();
        }
    }
}
