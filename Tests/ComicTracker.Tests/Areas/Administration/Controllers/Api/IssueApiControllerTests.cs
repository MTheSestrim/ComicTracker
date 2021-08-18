namespace ComicTracker.Tests.Areas.Administration.Controllers.Api
{
    using System.Linq;

    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Areas.Administration.Controllers.Api;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static ComicTracker.Tests.Data.Arc.ArcSample;
    using static ComicTracker.Tests.Data.Issue.IssueSample;
    using static ComicTracker.Tests.Data.Series.SeriesSample;
    using static ComicTracker.Tests.Data.Volume.VolumeSample;

    public class IssueApiControllerTests
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(4, 5)]
        [InlineData(3, 1500)]
        public void CreateIssuesShouldCreateGivenNumberOfIssuesAndAttachThemToSeries(
            int seriesId,
            int numberOfEntities)
            => MyController<IssueApiController>
                .Instance(controller => controller.WithData(SeriesWithId(seriesId)))
                // Act
                .Calling(c => c.CreateIssues(
                    new TemplateCreateApiRequestModel { SeriesId = seriesId, NumberOfEntities = numberOfEntities }))
                // Assert
                .ShouldHave()
                .Data(d => d.WithSet<Issue>(x => x.ToList().Count == numberOfEntities))
                .AndAlso()
                .ShouldReturn()
                .Result(numberOfEntities);

        [Fact]
        public void CreateIssuesShouldReturnBadRequestIfNumberOfEntitiesIsNegative()
            => MyController<IssueApiController>
                .Instance(controller => controller.WithData(SeriesWithId(2)))
                // Act
                .Calling(c => c.CreateIssues(
                    new TemplateCreateApiRequestModel { SeriesId = 2, NumberOfEntities = -2 }))
                // Assert
                .ShouldHave()
                .Data(d => d.WithSet<Arc>(x => x.ToList().Count == 0))
                .AndAlso()
                .ShouldReturn()
                .BadRequest();

        [Theory]
        [InlineData(1, 1, 5, 1)]
        [InlineData(4, 5, 10, 3)]
        [InlineData(3, 1, 10, 13)]
        public void AttachIssuesToSeriesRelatedEntityShouldAttachRangeOfArcsToGivenArc(
            int seriesId,
            int minRange,
            int maxRange,
            int arcId)
            => MyController<IssueApiController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(seriesId),
                        SREArcWithIdAndNumber(arcId, 1, seriesId))
                .WithData(TenSREIssuesWithIdsAndNumbers(seriesId)))
                // Act
                .Calling(c => c.AttachIssuesToSeriesRelatedEntity(
                    new AttachSRERequestModel
                    {
                        MinRange = minRange,
                        MaxRange = maxRange,
                        SeriesId = seriesId,
                        ParentId = arcId,
                        ParentTypeName = nameof(Arc),
                    }))
                // Assert
                .ShouldHave()
                .Data(d => d.WithSet<Arc>(x =>
                    x.Any(a =>
                        a.Id == arcId
                        && a.Issues.Count == maxRange - minRange + 1
                        && a.SeriesId == seriesId)))
                .AndAlso()
                .ShouldReturn()
                .Object(otb => otb.Passing(a => (int)a.Value == maxRange - minRange + 1));

        [Theory]
        [InlineData(1, 1, 5, 1)]
        [InlineData(4, 5, 10, 3)]
        [InlineData(3, 1, 10, 13)]
        public void AttachIssuesToSeriesRelatedEntityShouldAttachRangeOfArcsToGivenVolume(
            int seriesId,
            int minRange,
            int maxRange,
            int volumeId)
            => MyController<IssueApiController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(seriesId),
                        SREVolumeWithIdAndNumber(volumeId, 1, seriesId))
                .WithData(TenSREIssuesWithIdsAndNumbers(seriesId)))
                // Act
                .Calling(c => c.AttachIssuesToSeriesRelatedEntity(
                    new AttachSRERequestModel
                    {
                        MinRange = minRange,
                        MaxRange = maxRange,
                        SeriesId = seriesId,
                        ParentId = volumeId,
                        ParentTypeName = nameof(Volume),
                    }))
                // Assert
                .ShouldHave()
                .Data(d => d.WithSet<Volume>(x =>
                    x.Any(v =>
                        v.Id == volumeId
                        && v.Issues.Count == maxRange - minRange + 1
                        && v.SeriesId == seriesId)))
                .AndAlso()
                .ShouldReturn()
                .Object(otb => otb.Passing(a => (int)a.Value == maxRange - minRange + 1));

        [Fact]
        public void AttachIssuesToSeriesRelatedEntityShouldReturnBadRequestIfMinRangeIsLargerThanMaxRange()
            => MyController<IssueApiController>
                // Act
                .Calling(c => c.AttachIssuesToSeriesRelatedEntity(
                    new AttachSRERequestModel
                    {
                        MinRange = 5,
                        MaxRange = 4,
                        SeriesId = 3,
                        ParentId = 2,
                        ParentTypeName = nameof(Volume),
                    }))
                // Assert
                .ShouldReturn()
                .BadRequest();

        [Theory]
        [InlineData("Volume")]
        [InlineData("Arc")]
        public void AttachIssuesToSeriesRelatedEntityShouldReturnBadRequestIfGivenIncorrectRange(
            string parentTypeName)
            => MyController<IssueApiController>
                .Instance(controller => controller.WithData(SeriesWithId(1))
                    .WithData(TenSREArcsWithIdsAndNumbers(1))
                    .WithData(TenSREVolumesWithIdsAndNumbers(1)))
                // Act
                .Calling(c => c.AttachIssuesToSeriesRelatedEntity(
                    new AttachSRERequestModel
                    {
                        MinRange = 11,
                        MaxRange = 55,
                        ParentId = 1,
                        ParentTypeName = parentTypeName,
                        SeriesId = 1,
                    }))
                // Assert
                .ShouldReturn()
                .BadRequest("Incorrect issue range given or series given.");

        [Fact]
        public void AttachIssuesToSeriesRelatedEntityShouldReturnNullIfGivenWrongParentTypeName()
            => MyController<IssueApiController>
                .Instance(controller => controller.WithData(SeriesWithId(2))
                    .WithData(TenSREArcsWithIdsAndNumbers(2))
                    .WithData(TenSREVolumesWithIdsAndNumbers(2)))
                // Act
                .Calling(c => c.AttachIssuesToSeriesRelatedEntity(
                    new AttachSRERequestModel
                    {
                        MinRange = 3,
                        MaxRange = 5,
                        SeriesId = 2,
                        ParentId = 4,
                        ParentTypeName = nameof(Issue),
                    }))
                // Assert
                .ShouldReturn()
                .Null();

        [Fact]
        public void AttachIssuesToSeriesRelatedEntityShouldReturnNotFoundIfVolumeDoesNotExist()
            => MyController<IssueApiController>
                .Instance(controller => controller.WithData(SeriesWithId(3), SREVolumeWithIdAndNumber(2, 1, 3))
                    .WithData(TenSREIssuesWithIdsAndNumbers(3)))
                // Act
                .Calling(c => c.AttachIssuesToSeriesRelatedEntity(
                    new AttachSRERequestModel
                    {
                        MinRange = 3,
                        MaxRange = 5,
                        SeriesId = 3,
                        ParentId = 4,
                        ParentTypeName = nameof(Volume),
                    }))
                // Assert
                .ShouldReturn()
                .NotFound($"{nameof(Volume)} with given id 4 does not exist.");

        [Fact]
        public void AttachIssuesToSeriesRelatedEntityShouldReturnNotFoundIfArcDoesNotExist()
            => MyController<IssueApiController>
                .Instance(controller => controller.WithData(SeriesWithId(3), SREArcWithIdAndNumber(2, 1, 3))
                    .WithData(TenSREIssuesWithIdsAndNumbers(3)))
                // Act
                .Calling(c => c.AttachIssuesToSeriesRelatedEntity(
                    new AttachSRERequestModel
                    {
                        MinRange = 3,
                        MaxRange = 5,
                        SeriesId = 3,
                        ParentId = 4,
                        ParentTypeName = nameof(Arc),
                    }))
                // Assert
                .ShouldReturn()
                .NotFound($"{nameof(Arc)} with given id 4 does not exist.");

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void AttachIssuesToSeriesRelatedEntityShouldReturnBadRequestIfModelStateIsInvalid(
            int minRange,
            int maxRange)
            => MyController<IssueApiController>
                // Act
                .Calling(c => c.AttachIssuesToSeriesRelatedEntity(
                    new AttachSRERequestModel
                    {
                        MinRange = minRange,
                        MaxRange = maxRange,
                        SeriesId = 3,
                        ParentId = 4,
                        ParentTypeName = nameof(Volume),
                    }))
                // Assert
                .ShouldReturn()
                .BadRequest("Invalid model.");

        [Theory]
        [InlineData(1, 1, 5, 1)]
        [InlineData(4, 5, 10, 3)]
        [InlineData(3, 1, 10, 13)]
        public void DetachIssuesFromSeriesRelatedEntityShouldDetachRangeOfIssuesFromGivenVolume(
            int seriesId,
            int minRange,
            int maxRange,
            int volumeId)
            => MyController<IssueApiController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(seriesId),
                        SREVolumeWithIdAndNumberAndIssues(volumeId, 1, seriesId, minRange, maxRange)))
                // Act
                .Calling(c => c.DetachIssuesFromSeriesRelatedEntity(
                    new AttachSRERequestModel
                    {
                        MinRange = minRange,
                        MaxRange = maxRange,
                        SeriesId = seriesId,
                        ParentId = volumeId,
                        ParentTypeName = nameof(Volume),
                    }))
                // Assert
                .ShouldHave()
                .Data(d => d.WithSet<Volume>(x =>
                    x.Any(v =>
                        v.Id == volumeId
                        && v.Issues.Count == 0
                        && v.SeriesId == seriesId)))
                .AndAlso()
                .ShouldReturn()
                .Object(otb => otb.Passing(a => (int)a.Value == maxRange - minRange + 1));

        [Theory]
        [InlineData(1, 1, 5, 1)]
        [InlineData(4, 5, 10, 3)]
        [InlineData(3, 1, 10, 13)]
        public void DetachIssuesFromSeriesRelatedEntityShouldDetachRangeOfIssuesFromGivenArc(
            int seriesId,
            int minRange,
            int maxRange,
            int arcId)
            => MyController<IssueApiController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(seriesId),
                        SREArcWithIdAndNumberAndIssues(arcId, 1, seriesId, minRange, maxRange)))
                // Act
                .Calling(c => c.DetachIssuesFromSeriesRelatedEntity(
                    new AttachSRERequestModel
                    {
                        MinRange = minRange,
                        MaxRange = maxRange,
                        SeriesId = seriesId,
                        ParentId = arcId,
                        ParentTypeName = nameof(Arc),
                    }))
                // Assert
                .ShouldHave()
                .Data(d => d.WithSet<Arc>(x =>
                    x.Any(a =>
                        a.Id == arcId
                        && a.Issues.Count == 0
                        && a.SeriesId == seriesId)))
                .AndAlso()
                .ShouldReturn()
                .Object(otb => otb.Passing(a => (int)a.Value == maxRange - minRange + 1));

        [Fact]
        public void DetachIssuesFromSeriesRelatedEntityShouldReturnBadRequestIfMinRangeIsLargerThanMaxRange()
            => MyController<IssueApiController>
                // Act
                .Calling(c => c.DetachIssuesFromSeriesRelatedEntity(
                    new AttachSRERequestModel
                    {
                        MinRange = 5,
                        MaxRange = 4,
                        SeriesId = 3,
                        ParentId = 2,
                        ParentTypeName = nameof(Volume),
                    }))
                // Assert
                .ShouldReturn()
                .BadRequest("Invalid model.");

        [Fact]
        public void DetachIssuesFromSeriesRelatedEntityShouldReturnBadRequestIfGivenIncorrectRange()
            => MyController<IssueApiController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(1),
                        SREVolumeWithIdAndNumberAndIssues(1, 1, 1, 1, 10)))
                // Act
                .Calling(c => c.DetachIssuesFromSeriesRelatedEntity(
                    new AttachSRERequestModel
                    {
                        MinRange = 11,
                        MaxRange = 55,
                        ParentId = 1,
                        ParentTypeName = nameof(Volume),
                        SeriesId = 1,
                    }))
                // Assert
                .ShouldReturn()
                .BadRequest("Incorrect issue range or series given.");

        [Fact]
        public void DetachIssuesFromSeriesRelatedEntityShouldReturnBadRequestIfGivenIncorrectSeriesId()
            => MyController<IssueApiController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(1),
                        SREVolumeWithIdAndNumberAndIssues(1, 1, 1, 1, 10)))
                // Act
                .Calling(c => c.DetachIssuesFromSeriesRelatedEntity(
                    new AttachSRERequestModel
                    {
                        MinRange = 2,
                        MaxRange = 5,
                        ParentId = 1,
                        ParentTypeName = nameof(Volume),
                        SeriesId = 2,
                    }))
                // Assert
                .ShouldReturn()
                .BadRequest("Incorrect issue range or series given.");

        [Fact]
        public void DetachIssuesFromSeriesRelatedEntityShouldReturnNotFoundIfGivenIncorrectVolumeId()
            => MyController<IssueApiController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(1),
                        SREVolumeWithIdAndNumberAndIssues(1, 1, 1, 1, 10)))
                // Act
                .Calling(c => c.DetachIssuesFromSeriesRelatedEntity(
                    new AttachSRERequestModel
                    {
                        MinRange = 2,
                        MaxRange = 5,
                        ParentId = 2,
                        ParentTypeName = nameof(Volume),
                        SeriesId = 1,
                    }))
                // Assert
                .ShouldReturn()
                .NotFound($"{nameof(Volume)} with given id 2 does not exist.");

        [Fact]
        public void DetachIssuesFromSeriesRelatedEntityShouldReturnNotFoundIfGivenIncorrectArcId()
            => MyController<IssueApiController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(1),
                        SREArcWithIdAndNumberAndIssues(1, 1, 1, 1, 10)))
                // Act
                .Calling(c => c.DetachIssuesFromSeriesRelatedEntity(
                    new AttachSRERequestModel
                    {
                        MinRange = 2,
                        MaxRange = 5,
                        ParentId = 2,
                        ParentTypeName = nameof(Arc),
                        SeriesId = 1,
                    }))
                // Assert
                .ShouldReturn()
                .NotFound($"{nameof(Arc)} with given id 2 does not exist.");

        [Fact]
        public void DetachIssuesFromSeriesRelatedEntityShouldReturnNullIfGivenIncorrectParentType()
            => MyController<IssueApiController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(1),
                        SREVolumeWithIdAndNumberAndIssues(1, 1, 1, 2, 5)))
                // Act
                .Calling(c => c.DetachIssuesFromSeriesRelatedEntity(
                    new AttachSRERequestModel
                    {
                        MinRange = 3,
                        MaxRange = 5,
                        SeriesId = 1,
                        ParentId = 1,
                        ParentTypeName = nameof(Issue),
                    }))
                // Assert
                .ShouldReturn()
                .Null();

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void DetachIssuesFromSeriesRelatedEntityShouldReturnBadRequestIfModelStateIsInvalid(
            int minRange,
            int maxRange)
            => MyController<IssueApiController>
                // Act
                .Calling(c => c.DetachIssuesFromSeriesRelatedEntity(
                    new AttachSRERequestModel
                    {
                        MinRange = minRange,
                        MaxRange = maxRange,
                        SeriesId = 3,
                        ParentId = 4,
                        ParentTypeName = nameof(Volume),
                    }))
                // Assert
                .ShouldReturn()
                .BadRequest("Invalid model.");
    }
}
