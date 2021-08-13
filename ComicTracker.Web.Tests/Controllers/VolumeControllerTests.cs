namespace ComicTracker.Tests.Controllers
{
    using System;

    using ComicTracker.Services.Data.Volume.Models;
    using ComicTracker.Web.Controllers;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static ComicTracker.Common.CacheConstants;
    using static ComicTracker.Tests.Data.Volume.VolumeSample;

    public class VolumeControllerTests
    {
        [Theory]
        [InlineData(4)]
        [InlineData(2)]
        public void IndexShouldReturnViewWithCorrectModelAndData(int id)
            // Arrange
            => MyController<VolumeController>
                .Instance(controller => controller.WithData(VolumeWithId(id)))
                // Act
                .Calling(c => c.Index(id))
                // Assert
                .ShouldHave()
                .MemoryCache(cache => cache
                    .ContainingEntry(entry => entry
                        .WithKey(VolumeDetailsCacheKey + id.ToString())
                        .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromMinutes(1))
                        .WithValueOfType<VolumeDetailsServiceModel>()))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<VolumeDetailsServiceModel>()
                    .Passing(m => m.Id == id));
    }
}
