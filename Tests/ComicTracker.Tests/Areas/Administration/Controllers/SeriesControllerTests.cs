namespace ComicTracker.Tests.Areas.Administration.Controllers
{
    using System.Linq;

    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Web.Areas.Administration.Controllers;
    using ComicTracker.Web.ViewModels.Series;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static ComicTracker.Tests.Data.Genre.GenreSample;
    using static ComicTracker.Tests.Data.Series.SeriesSample;

    public class SeriesControllerTests
    {
        [Fact]
        public void CreateShouldReturnViewWithCorrectModelAndData()
            // Arrange
            => MyController<SeriesController>
                .Instance(controller => controller.WithData(TenGenresWithIds))
                // Act
                .Calling(c => c.Create())
                // Assert
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<CreateSeriesInputModel>()
                    .Passing(m => m.RetrievedGenres.Count() == TenGenresWithIds.Count()));

        [Theory]
        [InlineData("Batman: Year One")]
        [InlineData("Spider-man: Blue")]
        public void CreatePOSTShouldCreateSeriesAndRedirectToDetailsViewWithNewId(string title)
            // Arrange
            => MyController<SeriesController>
                .Instance(controller => controller.WithData(TenGenresWithIds))
                // Act
                .Calling(c => c.Create(new CreateSeriesInputModel
                {
                    Title = title,
                    Genres = Enumerable.Range(1, 5),
                }))
                // Assert
                .ShouldHave()
                .Data(d =>
                    d.WithSet<Series>(
                        x => x.FirstOrDefault(
                            s => s.Title == title && s.Id == 1 && s.Genres.Count == 5)))
                .AndAlso()
                .ShouldReturn()
                .Redirect("/Series/1");

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        public void EditShouldReturnEditingInfoForGivenSeries(int id)
            // Arrange
            => MyController<SeriesController>
                .Instance(controller => controller.WithData(SeriesWithId(id)).WithData(TenGenresWithIds))
                // Act
                .Calling(c => c.Edit(id))
                // Assert
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<EditSeriesInputModel>()
                    .Passing(m => m.Id == id && m.RetrievedGenres.Count() == TenGenresWithIds.Count()));

        [Fact]
        public void EditShouldReturnNotFoundIfSeriesDoesNotExist()
            // Arrange
            => MyController<SeriesController>
                .Instance(controller => controller.WithData(SeriesWithId(2)))
                // Act
                .Calling(c => c.Edit(3))
                // Assert
                .ShouldReturn()
                .NotFound(null);

        // MockSeriesEditingService is not needed
        [Theory]
        [InlineData(5, "Batman", "Bakuman")]
        [InlineData(3, "Spider-man", "Superman")]
        public void EditPOSTShouldEditSeriesAndRedirectToDetailsViewWithSameId(
            int id,
            string oldTitle,
            string newTitle)
            // Arrange
            => MyController<SeriesController>
                .Instance(controller => controller.WithData(SeriesWithIdAndTitle(id, oldTitle)).WithData(TenGenresWithIds))
                // Act
                .Calling(c => c.Edit(new EditSeriesInputModel
                {
                    Id = id,
                    Genres = Enumerable.Range(1, 5),
                    Title = newTitle,
                }))
                // Assert
                .ShouldHave()
                .Data(d =>
                    d.WithSet<Series>(
                        x => x.FirstOrDefault(
                            s => s.Id == id
                            && s.Title == newTitle
                            && s.Genres.Count == 5)))
                .AndAlso()
                .ShouldReturn()
                .Redirect($"/Series/{id}");

        [Fact]
        public void EditPOSTShouldReturnNotFoundIfSeriesDoesNotExist()
            // Arrange
            => MyController<SeriesController>
                .Instance(controller => controller.WithData(SeriesWithId(2)))
                // Act
                .Calling(c => c.Edit(new EditSeriesInputModel
                {
                    Id = 3,
                    Title = "Fullmetal Alchemist 2",
                }))
                // Assert
                .ShouldReturn()
                .NotFound(null);

        [Fact]
        public void DeleteShouldBeRestrictedForPOSTRequest()
            // Arrange
            => MyController<SeriesController>
                // Act
                .Calling(c => c.Delete(2))
                // Assert
                .ShouldHave()
                .ActionAttributes(a => a.RestrictingForHttpMethod(HttpMethod.Post));

        [Theory]
        [InlineData(2)]
        [InlineData(5)]
        public void DeleteShouldDeleteGivenSeriesAndRedirectToRelatedSeries(int id)
            // Arrange
            => MyController<SeriesController>
                .Instance(controller => controller.WithData(SeriesWithId(id)))
                // Act
                .Calling(c => c.Delete(id))
                // Assert
                .ShouldHave()
                .Data(d =>
                    d.WithSet<Series>(
                        x => !x.Any(s => s.Id == id)))
                .AndAlso()
                .ShouldReturn()
                .Redirect("/");

        [Fact]
        public void DeleteShouldReturnNotFoundWithNullValueIfSeriesDoesNotExist()
            // Arrange
            => MyController<SeriesController>
                // Act
                .Calling(c => c.Delete(2))
                // Assert
                .ShouldReturn()
                .NotFound();
    }
}
