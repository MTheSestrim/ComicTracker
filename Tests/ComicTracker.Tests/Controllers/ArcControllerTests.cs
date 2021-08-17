namespace ComicTracker.Tests.Controllers
{
    using System;

    using ComicTracker.Services.Data.Arc.Models;
    using ComicTracker.Web.Controllers;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static ComicTracker.Common.CacheConstants;
    using static ComicTracker.Tests.Data.Arc.ArcSample;

    public class ArcControllerTests
    {
        [Theory]
        [InlineData(4)]
        [InlineData(2)]
        public void IndexShouldReturnViewWithCorrectModelAndData(int id)

            // Arrange
            => MyController<ArcController>
                .Instance(controller => controller.WithData(ArcWithId(id)))
            // Act
                .Calling(c => c.Index(id))
            // Assert
                .ShouldHave()
                .MemoryCache(cache => cache
                    .ContainingEntry(entry => entry
                        .WithKey(ArcDetailsCacheKey + id.ToString())
                        .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromMinutes(1))
                        .WithValueOfType<ArcDetailsServiceModel>()))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<ArcDetailsServiceModel>()
                    .Passing(m => m.Id == id));

        [Fact]
        public void IndexShouldReturnNotFoundWithNullIfArcDoesNotExist()

            // Arrange
            => MyController<ArcController>
                .Instance(controller => controller.WithData(ArcWithId(2)))
                // Act
                .Calling(c => c.Index(3))
                // Assert
                .ShouldReturn()
                .NotFound(null);
    }
}
