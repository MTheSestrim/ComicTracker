namespace ComicTracker.Tests.Controllers.Api
{
    using System.Linq;

    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Controllers.Api;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static ComicTracker.Common.GlobalConstants;
    using static ComicTracker.Tests.Data.Series.SeriesSample;
    using static ComicTracker.Tests.Data.Volume.VolumeSample;

    public class VolumeApiControllerTests
    {
        [Fact]
        public void ScoreVolumeShouldBeRestrictedToHttpPutRequests()
            => MyController<VolumeApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(VolumeWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.ScoreVolume(new RateApiRequestModel { Id = 2, Score = 2 }))
                .ShouldHave()
                .ActionAttributes(attr => attr.RestrictingForHttpMethod(HttpMethod.Put));

        [Theory]
        [InlineData(2, 10)]
        [InlineData(4, 7)]
        [InlineData(8, 5)]
        [InlineData(10, 2)]
        [InlineData(12, 0)]
        public void ScoreVolumeShouldAddTheScoreOfTheVolumeForGivenUser(int id, int score)
            // Arrange
            => MyController<VolumeApiController>
                .Instance(controller => controller
                    .WithData(VolumeWithId(id))
                    .WithUser(u => u.WithIdentifier($"User{id}")))
                // Act
                .Calling(c => c.ScoreVolume(new RateApiRequestModel { Id = id, Score = score }))
                // Assert
                .ShouldHave()
                .Data(d => d.WithSet<Volume>(x => x.ToList()
                    .Any(v => v.UsersVolumes.Any(uv => uv.Score == score
                        && uv.UserId == $"User{id}"
                        && uv.VolumeId == id))))
                .AndAlso()
                .ShouldReturn()
                .Object(otb => otb.Passing(a => (int)a.Value == score));

        [Theory]
        [InlineData(2, 3, 7)]
        [InlineData(4, 0, 10)]
        [InlineData(8, 10, 0)]
        [InlineData(10, 5, 5)]
        public void ScoreVolumeShouldUpdateTheScoreOfTheVolumeForGivenUserIfOneIsAlreadyPresent(
            int id, 
            int oldScore, 
            int newScore)
            // Arrange
            => MyController<VolumeApiController>
                .Instance(controller => controller
                    .WithData(VolumeWithIdAndScore(id, oldScore, $"User{id}"))
                    .WithUser(u => u.WithIdentifier($"User{id}")))
                // Act
                .Calling(c => c.ScoreVolume(new RateApiRequestModel { Id = id, Score = newScore }))
                // Assert
                .ShouldHave()
                .Data(d => d.WithSet<Volume>(x => x.ToList()
                    .Any(v => v.UsersVolumes.Any(uv => uv.Score == newScore
                        && uv.UserId == $"User{id}"
                        && uv.VolumeId == id))))
                .AndAlso()
                .ShouldReturn()
                .Object(otb => otb.Passing(a => (int)a.Value == newScore));

        /*[Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(100)]
        public void CreateVolumesShouldCreateTemplateVolumesForGivenSeries(int numberOfEntities)
            // Arrange
            => MyController<VolumeApiController>
                .Instance(controller => controller
                    .WithData(SeriesWithId(4))
                    .WithUser(AdministratorRoleName))
                // Act
                .Calling(c => c.CreateVolumes(
                    new TemplateCreateApiRequestModel { SeriesId = 4, NumberOfEntities = numberOfEntities }))
                // Assert
                .ShouldHave()
                .Data(d => d.WithSet<Volume>(x => x.ToList().Count == numberOfEntities));*/
    }
}
