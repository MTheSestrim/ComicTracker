namespace ComicTracker.Tests.Controllers
{
    using ComicTracker.Common.Enums;
    using ComicTracker.Web.Controllers;
    using ComicTracker.Web.ViewModels.Home;

    using MyTested.AspNetCore.Mvc;
    using System;
    using Xunit;

    using static ComicTracker.Common.HomeConstants;
    using static ComicTracker.Tests.Data.Series.SeriesSample;

    public class HomeControllerTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void IndexShouldReturnPaginatedViewWithCorrectModelandData(int currentPage)
            // Arrange
            => MyController<HomeController>
                .Instance(controller => controller.WithData(FiftySeries))
                // Act
                .Calling(c => c.Index(new HomePageViewModel
                {
                    CurrentPage = currentPage,
                }))
                // Assert
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<HomePageViewModel>()
                    .Passing(m => m.TotalSeriesCount == 50
                    && (m.CurrentPage == currentPage)));

        [Fact]
        public void IndexShouldRedirectToFirstPageIfNegativeValueIsGiven()
        // Arrange
            => MyController<HomeController>
                .Instance(controller => controller.WithData(FiftySeries))
                // Act
                .Calling(c => c.Index(new HomePageViewModel
                {
                    CurrentPage = -1,
                }))
                // Assert
                .ShouldReturn()
                .RedirectToAction(
                    "Index",
                    new { Sorting = Sorting.Name, CurrentPage = 1, SearchTerm = string.Empty });

        [Fact]
        public void IndexShouldRedirectToLastPageIfCurrentPageValueOverflows()
            // Arrange
            => MyController<HomeController>
                .Instance(controller => controller.WithData(FiftySeries))
                // Act
                .Calling(c => c.Index(new HomePageViewModel
                {
                    CurrentPage = 12,
                }))
                // Assert
                .ShouldReturn()
                .RedirectToAction(
                    "Index",
                    new
                    {
                        Sorting = Sorting.Name,
                        CurrentPage = (int)Math.Ceiling(50D / SeriesPerPage),
                        SearchTerm = string.Empty,
                    });

        [Fact]
        public void PrivacyShouldReturnView()
            // Arrange
            => MyController<HomeController>
                .Instance()
                // Act
                .Calling(c => c.Privacy())
                // Assert
                .ShouldReturn()
                .View();
    }
}
