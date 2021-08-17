namespace ComicTracker.Tests.Controllers.Api
{
    using System.Linq;

    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Controllers.Api;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static ComicTracker.Tests.Data.Arc.ArcSample;

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

        [Fact]
        public void AddArcToListShouldBeRestrictedToHttpPutRequests()
            => MyController<ArcApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(ArcWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.AddArcToList(2))
                .ShouldHave()
                .ActionAttributes(attr => attr.RestrictingForHttpMethod(HttpMethod.Put));

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(10)]
        [InlineData(12)]
        public void AddArcToListShouldAddArcToUserList(int id)
            => MyController<ArcApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(ArcWithId(id))
                    .WithUser(u => u.WithIdentifier($"User{id}")))
                .Calling(c => c.AddArcToList(id))
                .ShouldHave()
                .Data(d => d.WithSet<Arc>(x => x.ToList()
                    .Any(a => a.UsersArcs.Any(ua => ua.UserId == $"User{id}" && ua.ArcId == id))))
                .AndAlso()
                .ShouldReturn()
                .NoContent();

        [Fact]
        public void AddArcToListShouldReturnNotFoundIfGivenWrongArcId()
            => MyController<ArcApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(ArcWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.AddArcToList(3))
                .ShouldReturn()
                .NotFound("Arc with given id does not exist.");

        [Fact]
        public void AddArcToListShouldReturnBadRequestIfUserAlreadyHasArcInList()
            => MyController<ArcApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(ArcWithIdAndScore(2, null, $"User{2}"))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.AddArcToList(2))
                .ShouldReturn()
                .BadRequest("User has already added given arc to their list.");

        [Fact]
        public void RemoveArcFromListShouldBeRestrictedToHttpDeleteRequests()
            => MyController<ArcApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(ArcWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.RemoveArcFromList(2))
                .ShouldHave()
                .ActionAttributes(attr => attr.RestrictingForHttpMethod(HttpMethod.Delete));

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(10)]
        [InlineData(12)]
        public void RemoveArcFromListShouldRemoveArcFromUserList(int id)
            => MyController<ArcApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(ArcWithIdAndScore(id, null, $"User{id}"))
                    .WithUser(u => u.WithIdentifier($"User{id}")))
                .Calling(c => c.RemoveArcFromList(id))
                .ShouldHave()
                .Data(d => d.WithSet<Arc>(x => x.ToList()
                    .All(a => a.UsersArcs.All(ua => ua.UserId != $"User{id}" && ua.ArcId != id))))
                .AndAlso()
                .ShouldReturn()
                .NoContent();

        [Fact]
        public void RemoveArcFromListShouldReturnNotFoundIfGivenWrongArcId()
            => MyController<ArcApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(ArcWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.RemoveArcFromList(3))
                .ShouldReturn()
                .NotFound("Arc with given id does not exist.");

        [Fact]
        public void RemoveArcFromListShouldReturnBadRequestIfUserDoesNotHaveArcInList()
            => MyController<ArcApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(ArcWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.RemoveArcFromList(2))
                .ShouldReturn()
                .BadRequest("User does not have given arc in their list.");

        /*[Theory]
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
                .Data(d => d.WithSet<Arc>(x => x.ToList().Count == numberOfEntities));*/
    }
}
