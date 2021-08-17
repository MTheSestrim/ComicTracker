namespace ComicTracker.Tests.Controllers.Api
{
    using System.Linq;

    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Controllers.Api;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static ComicTracker.Tests.Data.Issue.IssueSample;

    public class IssueApiControllerTests
    {
        [Fact]
        public void ScoreIssueShouldBeRestrictedToHttpPutRequests()
            => MyController<IssueApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(IssueWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.ScoreIssue(new RateApiRequestModel { Id = 2, Score = 2 }))
                .ShouldHave()
                .ActionAttributes(attr => attr.RestrictingForHttpMethod(HttpMethod.Put));

        [Theory]
        [InlineData(2, 10)]
        [InlineData(4, 7)]
        [InlineData(8, 5)]
        [InlineData(10, 2)]
        [InlineData(12, 0)]
        public void ScoreIssueShouldAddTheScoreOfTheIssueForGivenUser(int id, int score)
            // Arrange
            => MyController<IssueApiController>
                .Instance(controller => controller
                    .WithData(IssueWithId(id))
                    .WithUser(u => u.WithIdentifier($"User{id}")))
                // Act
                .Calling(c => c.ScoreIssue(new RateApiRequestModel { Id = id, Score = score }))
                // Assert
                .ShouldHave()
                .Data(d => d.WithSet<Issue>(x => x.ToList()
                    .Any(i => i.UsersIssues.Any(ui => ui.Score == score
                        && ui.UserId == $"User{id}"
                        && ui.IssueId == id))))
                .AndAlso()
                .ShouldReturn()
                .Object(otb => otb.Passing(a => (int)a.Value == score));

        [Theory]
        [InlineData(2, 3, 7)]
        [InlineData(4, 0, 10)]
        [InlineData(8, 10, 0)]
        [InlineData(10, 5, 5)]
        public void ScoreIssueShouldUpdateTheScoreOfTheIssueForGivenUserIfOneIsAlreadyPresent(
            int id,
            int oldScore,
            int newScore)
            // Arrange
            => MyController<IssueApiController>
                .Instance(controller => controller
                    .WithData(IssueWithIdAndScore(id, oldScore, $"User{id}"))
                    .WithUser(u => u.WithIdentifier($"User{id}")))
                // Act
                .Calling(c => c.ScoreIssue(new RateApiRequestModel { Id = id, Score = newScore }))
                // Assert
                .ShouldHave()
                .Data(d => d.WithSet<Issue>(x => x.ToList()
                    .Any(i => i.UsersIssues.Any(ui => ui.Score == newScore
                        && ui.UserId == $"User{id}"
                        && ui.IssueId == id))))
                .AndAlso()
                .ShouldReturn()
                .Object(otb => otb.Passing(a => (int)a.Value == newScore));

        [Fact]
        public void AddIssueToListShouldBeRestrictedToHttpPutRequests()
            => MyController<IssueApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(IssueWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.AddIssueToList(2))
                .ShouldHave()
                .ActionAttributes(attr => attr.RestrictingForHttpMethod(HttpMethod.Put));

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(10)]
        [InlineData(12)]
        public void AddIssueToListShouldAddIssueToUserList(int id)
            => MyController<IssueApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(IssueWithId(id))
                    .WithUser(u => u.WithIdentifier($"User{id}")))
                .Calling(c => c.AddIssueToList(id))
                .ShouldHave()
                .Data(d => d.WithSet<Issue>(x => x.ToList()
                    .Any(a => a.UsersIssues.Any(ua => ua.UserId == $"User{id}" && ua.IssueId == id))))
                .AndAlso()
                .ShouldReturn()
                .NoContent();

        [Fact]
        public void AddIssueToListShouldReturnNotFoundIfGivenWrongIssueId()
            => MyController<IssueApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(IssueWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.AddIssueToList(3))
                .ShouldReturn()
                .NotFound("Issue with given id does not exist.");

        [Fact]
        public void AddIssueToListShouldReturnBadRequestIfUserAlreadyHasIssueInList()
            => MyController<IssueApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(IssueWithIdAndScore(2, null, $"User{2}"))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.AddIssueToList(2))
                .ShouldReturn()
                .BadRequest("User has already added given issue to their list.");

        [Fact]
        public void RemoveIssueFromListShouldBeRestrictedToHttpDeleteRequests()
            => MyController<IssueApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(IssueWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.RemoveIssueFromList(2))
                .ShouldHave()
                .ActionAttributes(attr => attr.RestrictingForHttpMethod(HttpMethod.Delete));

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(10)]
        [InlineData(12)]
        public void RemoveIssueFromListShouldRemoveIssueFromUserList(int id)
            => MyController<IssueApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(IssueWithIdAndScore(id, null, $"User{id}"))
                    .WithUser(u => u.WithIdentifier($"User{id}")))
                .Calling(c => c.RemoveIssueFromList(id))
                .ShouldHave()
                .Data(d => d.WithSet<Issue>(x => x.ToList()
                    .All(a => a.UsersIssues.All(ua => ua.UserId != $"User{id}" && ua.IssueId != id))))
                .AndAlso()
                .ShouldReturn()
                .NoContent();

        [Fact]
        public void RemoveIssueFromListShouldReturnNotFoundIfGivenWrongIssueId()
            => MyController<IssueApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(IssueWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.RemoveIssueFromList(3))
                .ShouldReturn()
                .NotFound("Issue with given id does not exist.");

        [Fact]
        public void RemoveIssueFromListShouldReturnBadRequestIfUserDoesNotHaveIssueInList()
            => MyController<IssueApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(IssueWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.RemoveIssueFromList(2))
                .ShouldReturn()
                .BadRequest("User does not have given issue in their list.");

        /*[Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(100)]
        public void CreateIssuesShouldCreateTemplateIssuesForGivenSeries(int numberOfEntities)
            // Arrange
            => MyController<IssueApiController>
                .Instance(controller => controller
                    .WithData(SeriesWithId(4))
                    .WithUser(AdministratorRoleName))
                // Act
                .Calling(c => c.CreateIssues(
                    new TemplateCreateApiRequestModel { SeriesId = 4, NumberOfEntities = numberOfEntities }))
                // Assert
                .ShouldHave()
                .Data(d => d.WithSet<Issue>(x => x.ToList().Count == numberOfEntities));*/
    }
}
