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
    using static ComicTracker.Tests.Data.Issue.IssueSample;
    using static ComicTracker.Tests.Data.Series.SeriesSample;
    using static ComicTracker.Tests.Data.Volume.VolumeSample;

    public class IssueControllerTests
    {
        [Theory]
        [InlineData(4, 12)]
        [InlineData(2, 5)]
        public void CreateShouldReturnViewWithCorrectModelAndData(int seriesId, int number)
            // Arrange
            => MyController<IssueController>
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
        public void CreatePOSTShouldCreateIssueAndRedirectToDetailsViewWithNewId(int seriesId, int number)
            // Arrange
            => MyController<IssueController>
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
                    d.WithSet<Issue>(
                        x => x.FirstOrDefault(
                            x => x.SeriesId == seriesId && x.Number == number && x.Id == 1 && x.Genres.Count == 5)))
                .AndAlso()
                .ShouldReturn()
                .Redirect("/Issue/1");

        [Fact]
        public void CreatePOSTShouldReturnNotFoundIfSeriesDoesNotExist()
            // Arrange
            => MyController<IssueController>
                .Instance(controller => controller.WithData(SeriesWithId(2)))
                // Act
                .Calling(c => c.Create(new CreateSeriesRelatedEntityInputModel
                {
                    Number = 10,
                    SeriesId = 3,
                }))
                // Assert
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void CreatePOSTShouldReturnBadRequestIfIssueWithGivenNumberExists()
            // Arrange
            => MyController<IssueController>
                .Instance(controller => controller.WithData(SeriesWithId(2), SREIssueWithIdAndNumber(13, 4, 2)))
                // Act
                .Calling(c => c.Create(new CreateSeriesRelatedEntityInputModel
                {
                    Number = 4,
                    SeriesId = 2,
                }))
                // Assert
                .ShouldReturn()
                .BadRequest();

        [Theory]
        [InlineData(2, 4, 5)]
        [InlineData(4, 6, 8)]
        public void EditShouldReturnEditingInfoForGivenIssue(int id, int number, int seriesId)
            // Arrange
            => MyController<IssueController>
                .Instance(controller => controller.WithData(
                        SREIssueWithIdAndNumber(id, number, seriesId))
                    .WithData(TenGenresWithIds))
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
        public void EditShouldReturnNotFoundIfIssueDoesNotExist()
            // Arrange
            => MyController<IssueController>
                .Instance(controller => controller.WithData(IssueWithId(2)))
                // Act
                .Calling(c => c.Edit(3))
                // Assert
                .ShouldReturn()
                .NotFound();

        // MockIssueEditingService is not needed
        [Theory]
        [InlineData(5, 4, 12)]
        [InlineData(3, 2, 5)]
        public void EditPOSTShouldEditIssueAndRedirectToDetailsViewWithSameId(int id, int number, int seriesId)
            // Arrange
            => MyController<IssueController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(seriesId),
                        SREIssueWithIdAndNumber(id, number, seriesId))
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
                    d.WithSet<Issue>(
                        x => x.FirstOrDefault(
                            i => i.SeriesId == seriesId
                            && i.Number == number + 2
                            && i.Id == id
                            && i.Genres.Count == 5)))
                .AndAlso()
                .ShouldReturn()
                .Redirect($"/Issue/{id}");

        [Fact]
        public void EditPOSTShouldReturnNotFoundIfIssueDoesNotExist()
            // Arrange
            => MyController<IssueController>
                .Instance(controller => controller.WithData(SREIssueWithIdAndNumber(2, 5, 7)))
                // Act
                .Calling(c => c.Edit(new EditSeriesRelatedEntityInputModel
                {
                    Number = 8,
                    Id = 3,
                    SeriesId = 6,
                }))
                // Assert
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void EditPOSTShouldReturnNotFoundIfSeriesDoesNotExist()
            // Arrange
            => MyController<IssueController>
                .Instance(controller => controller.WithData(SREIssueWithIdAndNumber(2, 5, 5), SeriesWithId(7)))
                // Act
                .Calling(c => c.Edit(new EditSeriesRelatedEntityInputModel
                {
                    Number = 8,
                    Id = 2,
                    SeriesId = 7,
                }))
                // Assert
                .ShouldReturn()
                .NotFound();

        [Fact]
        public void EditPOSTShouldReturnBadRequestIfIssueWithGivenNumberExistsForGivenSeries()
           // Arrange
           => MyController<IssueController>
               .Instance(controller => controller.WithData(
                   SREIssueWithIdAndNumber(2, 5, 7),
                   SREIssueWithIdAndNumber(3, 6, 7),
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
               .BadRequest();

        [Fact]
        public void DeleteShouldBeRestrictedForPOSTRequest()
            // Arrange
            => MyController<IssueController>
                // Act
                .Calling(c => c.Delete(2))
                // Assert
                .ShouldHave()
                .ActionAttributes(a => a.RestrictingForHttpMethod(HttpMethod.Post));

        [Theory]
        [InlineData(2, 5)]
        [InlineData(5, 2)]
        public void DeleteShouldDeleteGivenIssueAndRedirectToRelatedSeries(int id, int seriesId)
            // Arrange
            => MyController<IssueController>
                .Instance(controller => controller.WithData(
                        SeriesWithId(seriesId),
                        SREIssueWithIdAndNumber(id, 1, seriesId)))
                // Act
                .Calling(c => c.Delete(id))
                // Assert
                .ShouldHave()
                .Data(d =>
                    d.WithSet<Issue>(
                        x => !x.Any(
                            i => i.SeriesId == seriesId
                            && i.Number == 1
                            && i.Id == id)))
                .AndAlso()
                .ShouldReturn()
                .Redirect($"/Series/{seriesId}");

        [Fact]
        public void DeleteShouldReturnNotFoundWithNullValueIfIssueDoesNotExist()
            // Arrange
            => MyController<IssueController>
                // Act
                .Calling(c => c.Delete(2))
                // Assert
                .ShouldReturn()
                .NotFound();

        [Theory]
        [InlineData(2, 5)]
        [InlineData(5, 2)]
        public void RemoveArcShouldRemoveArcFromIssueAndRedirectToIssueDetailsPage(int id, int arcId)
            // Arrange
            => MyController<IssueController>
                .Instance(controller => controller.WithData(
                        ArcWithId(arcId),
                        IssueWithIdAndArc(id, arcId)))
                // Act
                .Calling(c => c.RemoveArc(id))
                // Assert
                .ShouldHave()
                .Data(d =>
                    d.WithSet<Issue>(x => x.Any(i => i.ArcId == null && i.Id == id)))
                .AndAlso()
                .ShouldReturn()
                .Redirect($"/Issue/{id}");

        [Fact]
        public void RemoveArcShouldReturnNotFoundWithNullValueIfIssueDoesNotExist()
            // Arrange
            => MyController<IssueController>
                .Instance(controller => controller.WithData(IssueWithId(2)))
                // Act
                .Calling(c => c.RemoveArc(3))
                // Assert
                .ShouldReturn()
                .NotFound();

        [Theory]
        [InlineData(2, 5)]
        [InlineData(5, 2)]
        public void RemoveVolumeShouldRemoveVolumeFromIssueAndRedirectToIssueDetailsPage(int id, int volumeId)
            // Arrange
            => MyController<IssueController>
                .Instance(controller => controller.WithData(
                        VolumeWithId(volumeId),
                        IssueWithIdAndVolume(id, volumeId)))
                // Act
                .Calling(c => c.RemoveVolume(id))
                // Assert
                .ShouldHave()
                .Data(d =>
                    d.WithSet<Issue>(x => x.Any(i => i.VolumeId == null && i.Id == id)))
                .AndAlso()
                .ShouldReturn()
                .Redirect($"/Issue/{id}");

        [Fact]
        public void RemoveVolumeShouldReturnNotFoundWithNullValueIfIssueDoesNotExist()
            // Arrange
            => MyController<IssueController>
                .Instance(controller => controller.WithData(IssueWithId(2)))
                // Act
                .Calling(c => c.RemoveVolume(3))
                // Assert
                .ShouldReturn()
                .NotFound();
    }
}
