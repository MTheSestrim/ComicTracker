namespace ComicTracker.Tests.Controllers.Api
{
    using System.Linq;

    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Controllers.Api;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

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

        [Fact]
        public void AddVolumeToListShouldBeRestrictedToHttpPutRequests()
            => MyController<VolumeApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(VolumeWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.AddVolumeToList(2))
                .ShouldHave()
                .ActionAttributes(attr => attr.RestrictingForHttpMethod(HttpMethod.Put));

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(10)]
        [InlineData(12)]
        public void AddVolumeToListShouldAddVolumeToUserList(int id)
            => MyController<VolumeApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(VolumeWithId(id))
                    .WithUser(u => u.WithIdentifier($"User{id}")))
                .Calling(c => c.AddVolumeToList(id))
                .ShouldHave()
                .Data(d => d.WithSet<Volume>(x => x.ToList()
                    .Any(a => a.UsersVolumes.Any(ua => ua.UserId == $"User{id}" && ua.VolumeId == id))))
                .AndAlso()
                .ShouldReturn()
                .NoContent();

        [Fact]
        public void AddVolumeToListShouldReturnNotFoundIfGivenWrongVolumeId()
            => MyController<VolumeApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(VolumeWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.AddVolumeToList(3))
                .ShouldReturn()
                .NotFound("Volume with given id does not exist.");

        [Fact]
        public void AddVolumeToListShouldReturnBadRequestIfUserAlreadyHasVolumeInList()
            => MyController<VolumeApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(VolumeWithIdAndScore(2, null, $"User{2}"))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.AddVolumeToList(2))
                .ShouldReturn()
                .BadRequest("User has already added given volume to their list.");

        [Fact]
        public void RemoveVolumeFromListShouldBeRestrictedToHttpDeleteRequests()
            => MyController<VolumeApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(VolumeWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.RemoveVolumeFromList(2))
                .ShouldHave()
                .ActionAttributes(attr => attr.RestrictingForHttpMethod(HttpMethod.Delete));

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(10)]
        [InlineData(12)]
        public void RemoveVolumeFromListShouldRemoveVolumeFromUserList(int id)
            => MyController<VolumeApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(VolumeWithIdAndScore(id, null, $"User{id}"))
                    .WithUser(u => u.WithIdentifier($"User{id}")))
                .Calling(c => c.RemoveVolumeFromList(id))
                .ShouldHave()
                .Data(d => d.WithSet<Volume>(x => x.ToList()
                    .All(a => a.UsersVolumes.All(ua => ua.UserId != $"User{id}" && ua.VolumeId != id))))
                .AndAlso()
                .ShouldReturn()
                .NoContent();

        [Fact]
        public void RemoveVolumeFromListShouldReturnNotFoundIfGivenWrongVolumeId()
            => MyController<VolumeApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(VolumeWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.RemoveVolumeFromList(3))
                .ShouldReturn()
                .NotFound("Volume with given id does not exist.");

        [Fact]
        public void RemoveVolumeFromListShouldReturnBadRequestIfUserDoesNotHaveVolumeInList()
            => MyController<VolumeApiController>
                // Random data is given so that the action can be instantiated
                .Instance(controller => controller
                    .WithData(VolumeWithId(2))
                    .WithUser(u => u.WithIdentifier($"User{2}")))
                .Calling(c => c.RemoveVolumeFromList(2))
                .ShouldReturn()
                .BadRequest("User does not have given volume in their list.");

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
