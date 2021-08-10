namespace ComicTracker.Web.Tests
{
    using ComicTracker.Web.Controllers;
    using ComicTracker.Web.ViewModels.Home;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    using static ComicTracker.Common.CacheConstants;
    using static ComicTracker.Web.Tests.Data.Series.SeriesSample;

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
                .ShouldHave()
                .MemoryCache(cache => cache
                    .ContainingEntry(entry => entry
                        .WithKey(HomeCountCacheKey)
                        .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromMinutes(5))
                        .WithValue(50)))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<HomePageViewModel>()
                    .Passing(m => m.TotalSeriesCount == 50 
                    && (m.CurrentPage == currentPage)));
    }
}
