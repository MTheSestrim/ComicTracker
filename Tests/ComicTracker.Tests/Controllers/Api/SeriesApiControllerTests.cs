namespace ComicTracker.Tests.Controllers.Api
{
    using System.Linq;

    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Controllers.Api;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static ComicTracker.Tests.Data.Series.SeriesSample;

    public class SeriesApiControllerTests
    {
        [Fact]
        public void ScoreSeriesShouldBeRestrictedToHttpPutRequests()
            => MyController<SeriesApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(SeriesWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.ScoreSeries(new RateApiRequestModel { Id = 2, Score = 2 }))
                .ShouldHave()
                .ActionAttributes(attr => attr.RestrictingForHttpMethod(HttpMethod.Put));

        [Theory]
        [InlineData(2, 10)]
        [InlineData(4, 7)]
        [InlineData(8, 5)]
        [InlineData(10, 2)]
        [InlineData(12, 0)]
        public void ScoreSeriesShouldAddTheScoreOfTheSeriesForGivenUser(int id, int score)
            // Arrange
            => MyController<SeriesApiController>
                .Instance(controller => controller
                    .WithData(SeriesWithId(id))
                    .WithUser(u => u.WithIdentifier($"User{id}")))
                // Act
                .Calling(c => c.ScoreSeries(new RateApiRequestModel { Id = id, Score = score }))
                // Assert
                .ShouldHave()
                .Data(d => d.WithSet<Series>(x => x.ToList()
                    .Any(s => s.UsersSeries.Any(us => us.Score == score
                        && us.UserId == $"User{id}"
                        && us.SeriesId == id))))
                .AndAlso()
                .ShouldReturn()
                .Object(otb => otb.Passing(a => (int)a.Value == score));

        [Theory]
        [InlineData(2, 3, 7)]
        [InlineData(4, 0, 10)]
        [InlineData(8, 10, 0)]
        [InlineData(10, 5, 5)]
        public void ScoreSeriesShouldUpdateTheScoreOfTheSeriesForGivenUserIfOneIsAlreadyPresent(
            int id,
            int oldScore,
            int newScore)
            // Arrange
            => MyController<SeriesApiController>
                .Instance(controller => controller
                    .WithData(SeriesWithIdAndScore(id, oldScore, $"User{id}"))
                    .WithUser(u => u.WithIdentifier($"User{id}")))
                // Act
                .Calling(c => c.ScoreSeries(new RateApiRequestModel { Id = id, Score = newScore }))
                // Assert
                .ShouldHave()
                .Data(d => d.WithSet<Series>(x => x.ToList()
                    .Any(s => s.UsersSeries.Any(us => us.Score == newScore
                        && us.UserId == $"User{id}"
                        && us.SeriesId == id))))
                .AndAlso()
                .ShouldReturn()
                .Object(otb => otb.Passing(a => (int)a.Value == newScore));

        [Fact]
        public void AddSeriesToListShouldBeRestrictedToHttpPutRequests()
            => MyController<SeriesApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(SeriesWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.AddSeriesToList(2))
                .ShouldHave()
                .ActionAttributes(attr => attr.RestrictingForHttpMethod(HttpMethod.Put));

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(10)]
        [InlineData(12)]
        public void AddSeriesToListShouldAddSeriesToUserList(int id)
            => MyController<SeriesApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(SeriesWithId(id))
                    .WithUser(u => u.WithIdentifier($"User{id}")))
                .Calling(c => c.AddSeriesToList(id))
                .ShouldHave()
                .Data(d => d.WithSet<Series>(x => x.ToList()
                    .Any(a => a.UsersSeries.Any(ua => ua.UserId == $"User{id}" && ua.SeriesId == id))))
                .AndAlso()
                .ShouldReturn()
                .NoContent();

        [Fact]
        public void AddSeriesToListShouldReturnNotFoundIfGivenWrongSeriesId()
            => MyController<SeriesApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(SeriesWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.AddSeriesToList(3))
                .ShouldReturn()
                .NotFound("Series with given id does not exist.");

        [Fact]
        public void AddSeriesToListShouldReturnBadRequestIfUserAlreadyHasSeriesInList()
            => MyController<SeriesApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(SeriesWithIdAndScore(2, null, $"User{2}"))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.AddSeriesToList(2))
                .ShouldReturn()
                .BadRequest("User has already added given series to their list.");

        [Fact]
        public void RemoveSeriesFromListShouldBeRestrictedToHttpDeleteRequests()
            => MyController<SeriesApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(SeriesWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.RemoveSeriesFromList(2))
                .ShouldHave()
                .ActionAttributes(attr => attr.RestrictingForHttpMethod(HttpMethod.Delete));

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(10)]
        [InlineData(12)]
        public void RemoveSeriesFromListShouldRemoveSeriesFromUserList(int id)
            => MyController<SeriesApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(SeriesWithIdAndScore(id, null, $"User{id}"))
                    .WithUser(u => u.WithIdentifier($"User{id}")))
                .Calling(c => c.RemoveSeriesFromList(id))
                .ShouldHave()
                .Data(d => d.WithSet<Series>(x => x.ToList()
                    .All(a => a.UsersSeries.All(ua => ua.UserId != $"User{id}" && ua.SeriesId != id))))
                .AndAlso()
                .ShouldReturn()
                .NoContent();

        [Fact]
        public void RemoveSeriesFromListShouldReturnNotFoundIfGivenWrongSeriesId()
            => MyController<SeriesApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(SeriesWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.RemoveSeriesFromList(3))
                .ShouldReturn()
                .NotFound("Series with given id does not exist.");

        [Fact]
        public void RemoveSeriesFromListShouldReturnBadRequestIfUserDoesNotHaveSeriesInList()
            => MyController<SeriesApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(SeriesWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.RemoveSeriesFromList(2))
                .ShouldReturn()
                .BadRequest("User does not have given series in their list.");
    }
}
