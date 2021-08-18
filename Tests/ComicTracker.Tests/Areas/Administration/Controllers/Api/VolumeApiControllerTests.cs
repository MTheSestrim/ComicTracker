namespace ComicTracker.Tests.Areas.Administration.Controllers.Api
{
    using System.Linq;

    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Areas.Administration.Controllers.Api;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static ComicTracker.Tests.Data.Arc.ArcSample;
    using static ComicTracker.Tests.Data.Series.SeriesSample;
    using static ComicTracker.Tests.Data.Volume.VolumeSample;

    public class VolumeApiControllerTests
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(4, 5)]
        [InlineData(3, 1500)]
        public void CreateVolumesShouldCreateGivenNumberOfVolumesAndAttachThemToSeries(
            int seriesId, 
            int numberOfEntities)
            => MyController<VolumeApiController>
                .Instance(controller => controller.WithData(SeriesWithId(seriesId)))
                // Act
                .Calling(c => c.CreateVolumes(
                    new TemplateCreateApiRequestModel { SeriesId = seriesId, NumberOfEntities = numberOfEntities }))
                // Assert
                .ShouldHave()
                .Data(d => d.WithSet<Volume>(x => x.ToList().Count == numberOfEntities))
                .AndAlso()
                .ShouldReturn()
                .Result(numberOfEntities);

        [Fact]
        public void CreateVolumesShouldReturnBadRequestIfNumberOfEntitiesIsNegative()
            => MyController<ArcApiController>
                .Instance(controller => controller.WithData(SeriesWithId(2)))
                // Act
                .Calling(c => c.CreateArcs(
                    new TemplateCreateApiRequestModel { SeriesId = 2, NumberOfEntities = -2 }))
                // Assert
                .ShouldHave()
                .Data(d => d.WithSet<Volume>(x => x.ToList().Count == 0))
                .AndAlso()
                .ShouldReturn()
                .BadRequest();

        [Theory]
        [InlineData(1, 1, 5, 1)]
        [InlineData(4, 5, 10, 3)]
        [InlineData(3, 1, 10, 13)]
        public void AttachVolumesToArcShouldAttachRangeOfVolumesToGivenArc(
            int seriesId,
            int minRange,
            int maxRange,
            int arcId)
            => MyController<VolumeApiController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(seriesId),
                        SREArcWithIdAndNumber(arcId, 1, seriesId))
                .WithData(TenSREVolumesWithIdsAndNumbers(seriesId)))
                // Act
                .Calling(c => c.AttachVolumesToArc(
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
                        && a.ArcsVolumes.Count == maxRange - minRange + 1
                        && a.SeriesId == seriesId)))
                .AndAlso()
                .ShouldReturn()
                .Object(otb => otb.Passing(a => (int)a.Value == maxRange - minRange + 1));

        [Fact]
        public void AttachVolumesToArcShouldReturnBadRequestIfMinRangeIsLargerThanMaxRange()
            => MyController<VolumeApiController>
                // Act
                .Calling(c => c.AttachVolumesToArc(
                    new AttachSRERequestModel
                    {
                        MinRange = 5,
                        MaxRange = 4,
                        SeriesId = 3,
                        ParentId = 2,
                        ParentTypeName = nameof(Arc),
                    }))
                // Assert
                .ShouldReturn()
                .BadRequest();

        [Fact]
        public void AttachVolumesToArcShouldReturnBadRequestIfGivenIncorrectRange()
            => MyController<VolumeApiController>
                .Instance(controller => controller.WithData(SeriesWithId(1))
                    .WithData(TenSREVolumesWithIdsAndNumbers(1)))
                // Act
                .Calling(c => c.AttachVolumesToArc(
                    new AttachSRERequestModel
                    {
                        MinRange = 11,
                        MaxRange = 55,
                        ParentId = 1,
                        ParentTypeName = nameof(Arc),
                        SeriesId = 1,
                    }))
                // Assert
                .ShouldReturn()
                .BadRequest("Incorrect volume range or series given.");

        [Fact]
        public void AttachVolumesToArcShouldReturnNullIfGivenParentTypeNameOtherThanVolume()
            => MyController<VolumeApiController>
                .Instance(controller => controller.WithData(SeriesWithId(2))
                    .WithData(TenSREVolumesWithIdsAndNumbers(2)))
                // Act
                .Calling(c => c.AttachVolumesToArc(
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
        public void AttachVolumesToArcShouldReturnNotFoundIfArcDoesNotExist()
            => MyController<VolumeApiController>
                .Instance(controller => controller.WithData(SeriesWithId(3), SREArcWithIdAndNumber(2, 1, 3))
                    .WithData(TenSREVolumesWithIdsAndNumbers(3)))
                // Act
                .Calling(c => c.AttachVolumesToArc(
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
                .NotFound($"Arc with given id 4 does not exist.");

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void AttachVolumesToArcShouldReturnBadRequestIfModelStateIsInvalid(int minRange, int maxRange)
            => MyController<VolumeApiController>
                // Act
                .Calling(c => c.AttachVolumesToArc(
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
        public void DetachVolumesFromArcShouldDetachRangeOfVolumesFromGivenArc(
            int seriesId,
            int minRange,
            int maxRange,
            int arcId)
            => MyController<VolumeApiController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(seriesId),
                        SREArcWithIdAndNumberAndVolumes(arcId, 1, seriesId, minRange, maxRange))
                .WithData(TenSREVolumesWithIdsAndNumbers(seriesId)))
                // Act
                .Calling(c => c.DetachVolumesFromArc(
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
                        && a.ArcsVolumes.Count == 0
                        && a.SeriesId == seriesId)))
                .AndAlso()
                .ShouldReturn()
                .Object(otb => otb.Passing(a => (int)a.Value == maxRange - minRange + 1));

        [Fact]
        public void DetachVolumesFromArcShouldReturnBadRequestIfMinRangeIsLargerThanMaxRange()
            => MyController<VolumeApiController>
                // Act
                .Calling(c => c.DetachVolumesFromArc(
                    new AttachSRERequestModel
                    {
                        MinRange = 5,
                        MaxRange = 4,
                        SeriesId = 3,
                        ParentId = 2,
                        ParentTypeName = nameof(Arc),
                    }))
                // Assert
                .ShouldReturn()
                .BadRequest();

        [Fact]
        public void DetachVolumesFromArcShouldReturnBadRequestIfGivenIncorrectRange()
            => MyController<VolumeApiController>
                .Instance(controller => controller.WithData(SeriesWithId(1))
                    .WithData(SREArcWithIdAndNumberAndVolumes(1, 1, 1, 1, 10))
                    .WithData(TenSREVolumesWithIdsAndNumbers(1)))
                // Act
                .Calling(c => c.DetachVolumesFromArc(
                    new AttachSRERequestModel
                    {
                        MinRange = 11,
                        MaxRange = 55,
                        ParentId = 1,
                        ParentTypeName = nameof(Arc),
                        SeriesId = 1,
                    }))
                // Assert
                .ShouldReturn()
                .NotFound("Incorrect volume range, series or arc given.");

        [Fact]
        public void DetachVolumesFromArcShouldReturnBadRequestIfGivenIncorrectSeriesId()
            => MyController<VolumeApiController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(1),
                        SREArcWithIdAndNumberAndVolumes(1, 1, 1, 1, 10))
                    .WithData(TenSREVolumesWithIdsAndNumbers(1)))
                // Act
                .Calling(c => c.DetachVolumesFromArc(
                    new AttachSRERequestModel
                    {
                        MinRange = 2,
                        MaxRange = 5,
                        ParentId = 1,
                        ParentTypeName = nameof(Arc),
                        SeriesId = 2,
                    }))
                // Assert
                .ShouldReturn()
                .NotFound("Incorrect volume range, series or arc given.");

        [Fact]
        public void DetachVolumesFromArcShouldReturnNotFoundIfGivenIncorrectArcId()
            => MyController<VolumeApiController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(1),
                        SREArcWithIdAndNumberAndVolumes(1, 1, 1, 1, 10))
                    .WithData(TenSREVolumesWithIdsAndNumbers(1)))
                // Act
                .Calling(c => c.DetachVolumesFromArc(
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
                .NotFound("Incorrect volume range, series or arc given.");

        [Fact]
        public void DetachVolumesFromArcShouldReturnNullIfGivenParentTypeNameOtherThanVolume()
            => MyController<VolumeApiController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(1),
                        SREArcWithIdAndNumberAndVolumes(1, 1, 1, 2, 5))
                    .WithData(TenSREVolumesWithIdsAndNumbers(2)))
                // Act
                .Calling(c => c.DetachVolumesFromArc(
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
        public void DetachVolumesFromArcShouldReturnBadRequestIfModelStateIsInvalid(int minRange, int maxRange)
            => MyController<VolumeApiController>
                // Act
                .Calling(c => c.DetachVolumesFromArc(
                    new AttachSRERequestModel
                    {
                        MinRange = minRange,
                        MaxRange = maxRange,
                        SeriesId = 3,
                        ParentId = 4,
                        ParentTypeName = nameof(Arc),
                    }))
                // Assert
                .ShouldReturn()
                .BadRequest("Invalid model.");
    }
}

