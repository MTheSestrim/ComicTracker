namespace ComicTracker.Tests.Controllers
{
    using ComicTracker.Web.Controllers;
    using ComicTracker.Web.ViewModels.Home;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

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
    }
}
