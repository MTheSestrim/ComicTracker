namespace ComicTracker.Tests.Areas.Administration.Controllers
{
    using System.Linq;

    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Web.Areas.Administration.Controllers;
    using ComicTracker.Web.ViewModels.Entities;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static ComicTracker.Tests.Data.Genre.GenreSample;
    using static ComicTracker.Tests.Data.Series.SeriesSample;
    using static ComicTracker.Tests.Data.Volume.VolumeSample;

    public class VolumeControllerTests
    {
        [Theory]
        [InlineData(4, 12)]
        [InlineData(2, 5)]
        public void CreateShouldReturnViewWithCorrectModelAndData(int seriesId, int number)
            // Arrange
            => MyController<VolumeController>
                .Instance(controller => controller.WithData(SeriesWithId(seriesId)).WithData(TenGenresWithIds))
                // Act
                .Calling(c => c.Create(seriesId, number))
                // Assert
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CreateSeriesRelatedEntityInputModel>()
                    .Passing(m =>
                        m.RetrievedGenres.Count() == TenGenresWithIds.Count()
                        && m.SeriesId == seriesId
                        && m.Number == number));

        [Theory]
        [InlineData(4, 12)]
        [InlineData(2, 5)]
        public void CreatePOSTShouldCreateVolumeAndRedirectToDetailsViewWithNewId(int seriesId, int number)
            // Arrange
            => MyController<VolumeController>
                .Instance(controller => controller.WithData(SeriesWithId(seriesId)).WithData(TenGenresWithIds))
                // Act
                .Calling(c => c.Create(new CreateSeriesRelatedEntityInputModel
                {
                    Number = number,
                    SeriesId = seriesId,
                    Genres = Enumerable.Range(1, 5),
                }))
                // Assert
                .ShouldHave()
                .Data(d =>
                d.WithSet<Volume>(
                    x => x.FirstOrDefault(
                        v => v.SeriesId == seriesId && v.Number == number && v.Id == 1 && v.Genres.Count == 5)))
                .AndAlso()
                .ShouldReturn()
                .Redirect("/Volume/1");

        [Fact]
        public void CreatePOSTShouldReturnNotFoundIfSeriesDoesNotExist()
            // Arrange
            => MyController<VolumeController>
                .Instance(controller => controller.WithData(SeriesWithId(2)))
                // Act
                .Calling(c => c.Create(new CreateSeriesRelatedEntityInputModel
                {
                    Number = 10,
                    SeriesId = 3,
                }))
                // Assert
                .ShouldReturn()
                .NotFound("Series with given id 3 does not exist");

        [Fact]
        public void CreatePOSTShouldReturnBadRequestIfVolumeWithGivenNumberExists()
            // Arrange
            => MyController<VolumeController>
                .Instance(controller => controller.WithData(SeriesWithId(2), SREVolumeWithIdAndNumber(13, 4, 2)))
                // Act
                .Calling(c => c.Create(new CreateSeriesRelatedEntityInputModel
                {
                    Number = 4,
                    SeriesId = 2,
                }))
                // Assert
                .ShouldReturn()
                .BadRequest($"Cannot insert another {typeof(Volume).Name} with the same number");

        [Theory]
        [InlineData(2, 4, 5)]
        [InlineData(4, 6, 8)]
        public void EditShouldReturnEditingInfoForGivenVolume(int id, int number, int seriesId)
            // Arrange
            => MyController<VolumeController>
                .Instance(controller => controller.WithData(SREVolumeWithIdAndNumber(id, number, seriesId)).WithData(TenGenresWithIds))
                // Act
                .Calling(c => c.Edit(id))
                // Assert
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<EditSeriesRelatedEntityInputModel>()
                    .Passing(m => m.Id == id
                    && m.RetrievedGenres.Count() == TenGenresWithIds.Count()
                    && m.Number == number
                    && m.SeriesId == seriesId));

        [Fact]
        public void EditShouldReturnNotFoundIfVolumeDoesNotExist()
            // Arrange
            => MyController<VolumeController>
                .Instance(controller => controller.WithData(VolumeWithId(2)))
                // Act
                .Calling(c => c.Edit(3))
                // Assert
                .ShouldReturn()
                .NotFound(null);

        // MockVolumeEditingService is not needed
        [Theory]
        [InlineData(5, 4, 12)]
        [InlineData(3, 2, 5)]
        public void EditPOSTShouldEditVolumeAndRedirectToDetailsViewWithSameId(int id, int number, int seriesId)
            // Arrange
            => MyController<VolumeController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(seriesId),
                        SREVolumeWithIdAndNumber(id, number, seriesId))
                    .WithData(TenGenresWithIds))
                // Act
                .Calling(c => c.Edit(new EditSeriesRelatedEntityInputModel
                {
                    Number = number + 2,
                    Genres = Enumerable.Range(1, 5),
                    Id = id,
                    SeriesId = seriesId,
                }))
                // Assert
                .ShouldHave()
                .Data(d =>
                d.WithSet<Volume>(
                    x => x.FirstOrDefault(
                        x => x.SeriesId == seriesId
                        && x.Number == number + 2
                        && x.Id == id
                        && x.Genres.Count == 5)))
                .AndAlso()
                .ShouldReturn()
                .Redirect($"/Volume/{id}");

        [Fact]
        public void EditPOSTShouldReturnNotFoundIfVolumeDoesNotExist()
            // Arrange
            => MyController<VolumeController>
                .Instance(controller => controller.WithData(SREVolumeWithIdAndNumber(2, 5, 7)))
                // Act
                .Calling(c => c.Edit(new EditSeriesRelatedEntityInputModel
                {
                    Number = 8,
                    Id = 3,
                    SeriesId = 6,
                }))
                // Assert
                .ShouldReturn()
                .NotFound($"Volume with given id 3 does not exist");

        [Fact]
        public void EditPOSTShouldReturnNotFoundIfSeriesDoesNotExist()
            // Arrange
            => MyController<VolumeController>
                .Instance(controller => controller.WithData(SREVolumeWithIdAndNumber(2, 5, 5), SeriesWithId(7)))
                // Act
                .Calling(c => c.Edit(new EditSeriesRelatedEntityInputModel
                {
                    Number = 8,
                    Id = 2,
                    SeriesId = 7,
                }))
                // Assert
                .ShouldReturn()
                .NotFound("Wrong series id given for volume.");

        [Fact]
        public void EditPOSTShouldReturnBadRequestIfVolumeWithGivenNumberExistsForGivenSeries()
           // Arrange
           => MyController<VolumeController>
               .Instance(controller => controller.WithData(
                   SREVolumeWithIdAndNumber(2, 5, 7),
                   SREVolumeWithIdAndNumber(3, 6, 7),
                   SeriesWithId(7)))
               // Act
               .Calling(c => c.Edit(new EditSeriesRelatedEntityInputModel
               {
                   Number = 6,
                   Id = 2,
                   SeriesId = 7,
               }))
               // Assert
               .ShouldReturn()
               .BadRequest($"Cannot insert another {typeof(Volume).Name} with the same number");

        [Fact]
        public void DeleteShouldBeRestrictedForPOSTRequest()
            // Arrange
            => MyController<VolumeController>
                // Act
                .Calling(c => c.Delete(2))
                // Assert
                .ShouldHave()
                .ActionAttributes(a => a.RestrictingForHttpMethod(HttpMethod.Post));

        [Theory]
        [InlineData(2, 5)]
        [InlineData(5, 2)]
        public void DeleteShouldDeleteGivenVolumeAndRedirectToRelatedSeries(int id, int seriesId)
            // Arrange
            => MyController<VolumeController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(seriesId),
                        SREVolumeWithIdAndNumber(id, 1, seriesId)))
                // Act
                .Calling(c => c.Delete(id))
                // Assert
                .ShouldHave()
                .Data(d =>
                d.WithSet<Volume>(
                    x => !x.Any(
                        x => x.SeriesId == seriesId
                        && x.Number == 1
                        && x.Id == id)))
                .AndAlso()
                .ShouldReturn()
                .Redirect($"/Series/{seriesId}");

        [Fact]
        public void DeleteShouldReturnNotFoundWithNullValueIfVolumeDoesNotExist()
            // Arrange
            => MyController<VolumeController>
                // Act
                .Calling(c => c.Delete(2))
                // Assert
                .ShouldReturn()
                .NotFound(null);
    }
}
