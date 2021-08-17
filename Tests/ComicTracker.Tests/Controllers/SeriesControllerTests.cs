namespace ComicTracker.Tests.Controllers
{
    using System;

    using ComicTracker.Services.Data.Series.Models;
    using ComicTracker.Web.Controllers;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static ComicTracker.Common.CacheConstants;
    using static ComicTracker.Tests.Data.Series.SeriesSample;

    public class SeriesControllerTests
    {
        [Theory]
        [InlineData(4)]
        [InlineData(2)]
        public void IndexShouldReturnViewWithCorrectModelAndData(int id)
            // Arrange
            => MyController<SeriesController>
                .Instance(controller => controller.WithData(SeriesWithId(id)))
                // Act
                .Calling(c => c.Index(id))
                // Assert
                .ShouldHave()
                .MemoryCache(cache => cache
                    .ContainingEntry(entry => entry
                        .WithKey(SeriesDetailsCacheKey + id.ToString())
                        .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromMinutes(3))
                        .WithValueOfType<SeriesDetailsServiceModel>()))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<SeriesDetailsServiceModel>()
                    .Passing(m => m.Id == id));

        [Fact]
        public void IndexShouldReturnNotFoundWithNullIfSeriesDoesNotExist()

            // Arrange
            => MyController<SeriesController>
                .Instance(controller => controller.WithData(SeriesWithId(2)))
                // Act
                .Calling(c => c.Index(3))
                // Assert
                .ShouldReturn()
                .NotFound(null);
    }
}
