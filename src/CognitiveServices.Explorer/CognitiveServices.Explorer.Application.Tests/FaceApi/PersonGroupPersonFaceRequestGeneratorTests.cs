using CognitiveServices.Explorer.Application.FaceApi;
using FluentAssertions;
using Snapshooter.Xunit;
using System;
using Xunit;

namespace CognitiveServices.Explorer.Application.Tests.FaceApi
{
    public class PersonGroupPersonFaceRequestGeneratorTests
    {
        [Fact]
        public void ShouldGenerateAddBinary()
        {
            PersonGroupPersonFaceRequestGenerator
                .Add("default", Guid.Empty.ToString(), new byte[0])
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateAddUrl()
        {
            PersonGroupPersonFaceRequestGenerator
                .Add("default", Guid.Empty.ToString(), "https://image.url")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateAddUrlWithAttributes()
        {
            PersonGroupPersonFaceRequestGenerator
                .Add("default", Guid.Empty.ToString(), "https://image.url", FaceRequestGenerator.DefaultDetectionModel, @"{""image"":""data""}")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateGet()
        {
            PersonGroupPersonFaceRequestGenerator
                .Get("default", Guid.Empty.ToString(), new Guid("4684f729-57d2-4512-907d-b9e0f0213bdb").ToString())
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateUpdate()
        {
            PersonGroupPersonFaceRequestGenerator
                .Update("default", Guid.Empty.ToString(), new Guid("4684f729-57d2-4512-907d-b9e0f0213bdb").ToString(), @"{}")
                .Should()
                .MatchSnapshot();
        }

        [Fact]
        public void ShouldGenerateDelete()
        {
            PersonGroupPersonFaceRequestGenerator
                .Delete("default", Guid.Empty.ToString(), new Guid("4684f729-57d2-4512-907d-b9e0f0213bdb").ToString())
                .Should()
                .MatchSnapshot();
        }
    }
}
