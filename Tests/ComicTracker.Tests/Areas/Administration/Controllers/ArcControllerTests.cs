namespace ComicTracker.Tests.Areas.Administration.Controllers
{
    using System.Linq;

    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Web.Areas.Administration.Controllers;
    using ComicTracker.Web.ViewModels.Entities;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static ComicTracker.Tests.Data.Arc.ArcSample;
    using static ComicTracker.Tests.Data.Genre.GenreSample;
    using static ComicTracker.Tests.Data.Series.SeriesSample;

    public class ArcControllerTests
    {
        [Theory]
        [InlineData(4, 12)]
        [InlineData(2, 5)]
        public void CreateShouldReturnViewWithCorrectModelAndData(int seriesId, int number)
            // Arrange
            => MyController<ArcController>
                .Instance(controller => controller.WithData(SeriesWithId(seriesId)).WithData(TenGenresWithIds))
                // Act
                .Calling(c => c.Create(seriesId, number))
                // Assert
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CreateSeriesRelatedEntityInputModel>()
                    .Passing(m => m.RetrievedGenres.Count() == TenGenresWithIds.Count() && m.SeriesId == seriesId && m.Number == number));

        [Theory]
        [InlineData(4, 12)]
        [InlineData(2, 5)]
        public void CreatePOSTShouldCreateArcAndRedirectToDetailsViewWithNewId(int seriesId, int number)
            // Arrange
            => MyController<ArcController>
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
                d.WithSet<Arc>(
                    x => x.FirstOrDefault(
                        x => x.SeriesId == seriesId && x.Number == number && x.Id == 1 && x.Genres.Count == 5)))
                .AndAlso()
                .ShouldReturn()
                .Redirect("/Arc/1");

        [Fact]
        public void CreatePOSTShouldReturnNotFoundIfSeriesDoesNotExist()
            // Arrange
            => MyController<ArcController>
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
        public void CreatePOSTShouldReturnBadRequestIfArcWithGivenNumberExists()
            // Arrange
            => MyController<ArcController>
                .Instance(controller => controller.WithData(SeriesWithId(2), SREArcWithIdAndNumber(13, 4, 2)))
                // Act
                .Calling(c => c.Create(new CreateSeriesRelatedEntityInputModel
                {
                    Number = 4,
                    SeriesId = 2,
                }))
                // Assert
                .ShouldReturn()
                .BadRequest($"Cannot insert another {typeof(Arc).Name} with the same number");

        [Theory]
        [InlineData(2, 4, 5)]
        [InlineData(4, 6, 8)]
        public void EditShouldReturnEditingInfoForGivenArc(int id, int number, int seriesId)
            // Arrange
            => MyController<ArcController>
                .Instance(controller => controller.WithData(SREArcWithIdAndNumber(id, number, seriesId)).WithData(TenGenresWithIds))
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
        public void EditShouldReturnNotFoundIfArcDoesNotExist()
            // Arrange
            => MyController<ArcController>
                .Instance(controller => controller.WithData(ArcWithId(2)))
                // Act
                .Calling(c => c.Edit(3))
                // Assert
                .ShouldReturn()
                .NotFound(null);

        // MockArcEditingService is not needed
        [Theory]
        [InlineData(5, 4, 12)]
        [InlineData(3, 2, 5)]
        public void EditPOSTShouldEditArcAndRedirectToDetailsViewWithSameId(int id, int number, int seriesId)
            // Arrange
            => MyController<ArcController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(seriesId),
                        SREArcWithIdAndNumber(id, number, seriesId))
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
                d.WithSet<Arc>(
                    x => x.FirstOrDefault(
                        x => x.SeriesId == seriesId
                        && x.Number == number + 2
                        && x.Id == id
                        && x.Genres.Count == 5)))
                .AndAlso()
                .ShouldReturn()
                .Redirect($"/Arc/{id}");

        [Fact]
        public void EditPOSTShouldReturnNotFoundIfArcDoesNotExist()
            // Arrange
            => MyController<ArcController>
                .Instance(controller => controller.WithData(SREArcWithIdAndNumber(2, 5, 7)))
                // Act
                .Calling(c => c.Edit(new EditSeriesRelatedEntityInputModel
                {
                    Number = 8,
                    Id = 3,
                    SeriesId = 6,
                }))
                // Assert
                .ShouldReturn()
                .NotFound($"Arc with given id 3 does not exist");

        [Fact]
        public void EditPOSTShouldReturnNotFoundIfSeriesDoesNotExist()
            // Arrange
            => MyController<ArcController>
                .Instance(controller => controller.WithData(SREArcWithIdAndNumber(2, 5, 5), SeriesWithId(7)))
                // Act
                .Calling(c => c.Edit(new EditSeriesRelatedEntityInputModel
                {
                    Number = 8,
                    Id = 2,
                    SeriesId = 7,
                }))
                // Assert
                .ShouldReturn()
                .NotFound("Wrong series id given for arc.");

        [Fact]
        public void EditPOSTShouldReturnBadRequestIfArcWithGivenNumberExistsForGivenSeries()
           // Arrange
           => MyController<ArcController>
               .Instance(controller => controller.WithData(
                   SREArcWithIdAndNumber(2, 5, 7),
                   SREArcWithIdAndNumber(3, 6, 7),
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
               .BadRequest($"Cannot insert another {typeof(Arc).Name} with the same number");

        [Fact]
        public void DeleteShouldBeRestrictedForPOSTRequest()
            // Arrange
            => MyController<ArcController>
                // Act
                .Calling(c => c.Delete(2))
                // Assert
                .ShouldHave()
                .ActionAttributes(a => a.RestrictingForHttpMethod(HttpMethod.Post));

        [Theory]
        [InlineData(2, 5)]
        [InlineData(5, 2)]
        public void DeleteShouldDeleteGivenArcAndRedirectToRelatedSeries(int id, int seriesId)
            // Arrange
            => MyController<ArcController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(seriesId),
                        SREArcWithIdAndNumber(id, 1, seriesId)))
                // Act
                .Calling(c => c.Delete(id))
                // Assert
                .ShouldHave()
                .Data(d =>
                d.WithSet<Arc>(
                    x => !x.Any(
                        a => a.SeriesId == seriesId
                        && a.Number == 1
                        && a.Id == id)))
                .AndAlso()
                .ShouldReturn()
                .Redirect($"/Series/{seriesId}");

        [Fact]
        public void DeleteShouldReturnNotFoundWithNullValueIfArcDoesNotExist()
            // Arrange
            => MyController<ArcController>
                // Act
                .Calling(c => c.Delete(2))
                // Assert
                .ShouldReturn()
                .NotFound(null);
    }
}
