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

    public class ArcApiControllerTests
    {
        [Theory]
        [InlineData(1, 1)]
        [InlineData(4, 5)]
        [InlineData(3, 1500)]
        public void CreateArcsShouldCreateGivenNumberOfArcsAndAttachThemToSeries(int seriesId, int numberOfEntities)
            => MyController<ArcApiController>
                .Instance(controller => controller.WithData(SeriesWithId(seriesId)))
                // Act
                .Calling(c => c.CreateArcs(
                    new TemplateCreateApiRequestModel { SeriesId = seriesId, NumberOfEntities = numberOfEntities }))
                // Assert
                .ShouldHave()
                .Data(d => d.WithSet<Arc>(x => x.ToList().Count == numberOfEntities))
                .AndAlso()
                .ShouldReturn()
                .Result(numberOfEntities);

        [Fact]
        public void CreateArcsShouldReturnBadRequestIfNumberOfEntitiesIsNegative()
            => MyController<ArcApiController>
                .Instance(controller => controller.WithData(SeriesWithId(2)))
                // Act
                .Calling(c => c.CreateArcs(
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
        public void AttachArcsToVolumeShouldAttachRangeOfArcsToGivenVolume(
            int seriesId,
            int minRange,
            int maxRange,
            int volumeId)
            => MyController<ArcApiController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(seriesId),
                        SREVolumeWithIdAndNumber(volumeId, 1, seriesId))
                .WithData(TenSREArcsWithIdsAndNumbers(seriesId)))
                // Act
                .Calling(c => c.AttachArcsToVolume(
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
                        && v.ArcsVolumes.Count == maxRange - minRange + 1
                        && v.SeriesId == seriesId)))
                .AndAlso()
                .ShouldReturn()
                .Object(otb => otb.Passing(a => (int)a.Value == maxRange - minRange + 1));

        [Fact]
        public void AttachArcsToVolumeShouldReturnBadRequestIfMinRangeIsLargerThanMaxRange()
            => MyController<ArcApiController>
                // Act
                .Calling(c => c.AttachArcsToVolume(
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

        [Fact]
        public void AttachArcsToVolumeShouldReturnBadRequestIfGivenIncorrectRange()
            => MyController<ArcApiController>
                .Instance(controller => controller.WithData(SeriesWithId(1))
                    .WithData(TenSREArcsWithIdsAndNumbers(1)))
                // Act
                .Calling(c => c.AttachArcsToVolume(
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
                .BadRequest("Incorrect arc range or series given.");

        [Fact]
        public void AttachArcsToVolumeShouldReturnNullIfGivenParentTypeNameOtherThanVolume()
            => MyController<ArcApiController>
                .Instance(controller => controller.WithData(SeriesWithId(2))
                    .WithData(TenSREArcsWithIdsAndNumbers(2)))
                // Act
                .Calling(c => c.AttachArcsToVolume(
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
        public void AttachArcsToVolumeShouldReturnNotFoundIfVolumeDoesNotExist()
            => MyController<ArcApiController>
                .Instance(controller => controller.WithData(SeriesWithId(3), SREVolumeWithIdAndNumber(2, 1, 3))
                    .WithData(TenSREArcsWithIdsAndNumbers(3)))
                // Act
                .Calling(c => c.AttachArcsToVolume(
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
                .NotFound($"Volume with given id 4 does not exist.");

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        public void AttachArcsToVolumeShouldReturnBadRequestIfModelStateIsInvalid(int minRange, int maxRange)
            => MyController<ArcApiController>
                // Act
                .Calling(c => c.AttachArcsToVolume(
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
        public void DetachArcsFromVolumeShouldDetachRangeOfArcsFromGivenVolume(
            int seriesId,
            int minRange,
            int maxRange,
            int volumeId)
            => MyController<ArcApiController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(seriesId),
                        SREVolumeWithIdAndNumberAndArcs(volumeId, 1, seriesId, minRange, maxRange))
                .WithData(TenSREArcsWithIdsAndNumbers(seriesId)))
                // Act
                .Calling(c => c.DetachArcsFromVolume(
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
                        && v.ArcsVolumes.Count == 0
                        && v.SeriesId == seriesId)))
                .AndAlso()
                .ShouldReturn()
                .Object(otb => otb.Passing(a => (int)a.Value == maxRange - minRange + 1));

        [Fact]
        public void DetachArcsFromVolumeShouldReturnBadRequestIfMinRangeIsLargerThanMaxRange()
            => MyController<ArcApiController>
                // Act
                .Calling(c => c.DetachArcsFromVolume(
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

        [Fact]
        public void DetachArcsFromVolumeShouldReturnBadRequestIfGivenIncorrectRange()
            => MyController<ArcApiController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(1),
                        SREVolumeWithIdAndNumberAndArcs(1, 1, 1, 1, 10))
                    .WithData(TenSREArcsWithIdsAndNumbers(1)))
                // Act
                .Calling(c => c.DetachArcsFromVolume(
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
                .NotFound("Incorrect arc range, series or volume given.");

        [Fact]
        public void DetachArcsFromVolumeShouldReturnBadRequestIfGivenIncorrectSeriesId()
            => MyController<ArcApiController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(1),
                        SREVolumeWithIdAndNumberAndArcs(1, 1, 1, 1, 10))
                    .WithData(TenSREArcsWithIdsAndNumbers(1)))
                // Act
                .Calling(c => c.DetachArcsFromVolume(
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
                .NotFound("Incorrect arc range, series or volume given.");

        [Fact]
        public void DetachArcsFromVolumeShouldReturnBadRequestIfGivenIncorrectVolumeId()
            => MyController<ArcApiController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(1),
                        SREVolumeWithIdAndNumberAndArcs(1, 1, 1, 1, 10))
                    .WithData(TenSREArcsWithIdsAndNumbers(1)))
                // Act
                .Calling(c => c.DetachArcsFromVolume(
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
                .NotFound("Incorrect arc range, series or volume given.");

        [Fact]
        public void DetachArcsFromVolumeShouldReturnNullIfGivenParentTypeNameOtherThanVolume()
            => MyController<ArcApiController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(1),
                        SREVolumeWithIdAndNumberAndArcs(1, 1, 1, 2, 5))
                    .WithData(TenSREArcsWithIdsAndNumbers(2)))
                // Act
                .Calling(c => c.DetachArcsFromVolume(
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
        public void DetachArcsFromVolumeShouldReturnBadRequestIfModelStateIsInvalid(int minRange, int maxRange)
            => MyController<ArcApiController>
                // Act
                .Calling(c => c.DetachArcsFromVolume(
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
