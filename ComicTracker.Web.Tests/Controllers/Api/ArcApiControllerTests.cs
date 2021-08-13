namespace ComicTracker.Tests.Controllers.Api
{
    using System.Linq;

    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Controllers.Api;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static ComicTracker.Common.GlobalConstants;
    using static ComicTracker.Tests.Data.Arc.ArcSample;
    using static ComicTracker.Tests.Data.Series.SeriesSample;

    public class ArcApiControllerTests
    {
        [Fact]
        public void ScoreArcShouldBeRestrictedToHttpPutRequests()
            => MyController<ArcApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(ArcWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.ScoreArc(new RateApiRequestModel { Id = 2, Score = 2 }))
                .ShouldHave()
                .ActionAttributes(attr => attr.RestrictingForHttpMethod(HttpMethod.Put));

        [Theory]
        [InlineData(2, 10)]
        [InlineData(4, 7)]
        [InlineData(8, 5)]
        [InlineData(10, 2)]
        [InlineData(12, 0)]
        public void ScoreArcShouldAddTheScoreOfTheArcForGivenUser(int id, int score)
            // Arrange
            => MyController<ArcApiController>
                .Instance(controller => controller
                    .WithData(ArcWithId(id))
                    .WithUser(u => u.WithIdentifier($"User{id}")))
                // Act
                .Calling(c => c.ScoreArc(new RateApiRequestModel { Id = id, Score = score }))
                // Assert
                .ShouldHave()
                .Data(d => d.WithSet<Arc>(x => x.ToList()
                    .Any(a => a.UsersArcs.Any(ua => ua.Score == score
                        && ua.UserId == $"User{id}"
                        && ua.ArcId == id))))
                .AndAlso()
                .ShouldReturn()
                .Object(otb => otb.Passing(a => (int)a.Value == score));

        [Theory]
        [InlineData(2, 3, 7)]
        [InlineData(4, 0, 10)]
        [InlineData(8, 10, 0)]
        [InlineData(10, 5, 5)]
        public void ScoreArcShouldUpdateTheScoreOfTheArcForGivenUserIfOneIsAlreadyPresent(
            int id,
            int oldScore,
            int newScore)
            // Arrange
            => MyController<ArcApiController>
                .Instance(controller => controller
                    .WithData(ArcWithIdAndScore(id, oldScore, $"User{id}"))
                    .WithUser(u => u.WithIdentifier($"User{id}")))
                // Act
                .Calling(c => c.ScoreArc(new RateApiRequestModel { Id = id, Score = newScore }))
                // Assert
                .ShouldHave()
                .Data(d => d.WithSet<Arc>(x => x.ToList()
                    .Any(a => a.UsersArcs.Any(ua => ua.Score == newScore
                        && ua.UserId == $"User{id}"
                        && ua.ArcId == id))))
                .AndAlso()
                .ShouldReturn()
                .Object(otb => otb.Passing(a => (int)a.Value == newScore));

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(100)]
        public void CreateArcsShouldCreateTemplateArcsForGivenSeries(int numberOfEntities)
            // Arrange
            => MyController<ArcApiController>
                .Instance(controller => controller
                    .WithData(SeriesWithId(4))
                    .WithUser(AdministratorRoleName))
                // Act
                .Calling(c => c.CreateArcs(
                    new TemplateCreateApiRequestModel { SeriesId = 4, NumberOfEntities = numberOfEntities }))
                // Assert
                .ShouldHave()
                .Data(d => d.WithSet<Arc>(x => x.ToList().Count == numberOfEntities));
    }
}
