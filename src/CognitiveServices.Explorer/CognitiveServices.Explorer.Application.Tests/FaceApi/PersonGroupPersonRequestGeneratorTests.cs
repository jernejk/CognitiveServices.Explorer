using CognitiveServices.Explorer.Application.FaceApi;
using FluentAssertions;
using Snapshooter.Xunit;
using System;
using Xunit;

namespace CognitiveServices.Explorer.Application.Tests.FaceApi
{
    public class PersonGroupPersonRequestGeneratorTests
    {
        [Fact]
        public void ShouldGenerateCreate()
        {
            PersonGroupPersonRequestGenerator
                .Create("default", "User", @"{""test"":""data""}")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateUpdate()
        {
            PersonGroupPersonRequestGenerator
                .Update("default", Guid.Empty.ToString(), "User2")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateDelete()
        {
            PersonGroupPersonRequestGenerator
                .Delete("default", Guid.Empty.ToString())
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateGet()
        {
            PersonGroupPersonRequestGenerator
                .Get("default", Guid.Empty.ToString())
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateList()
        {
            PersonGroupPersonRequestGenerator
                .List("default")
                .Should()
                .MatchSnapshot();
        }
    }
}
