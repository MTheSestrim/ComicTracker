namespace ComicTracker.Tests.Controllers
{
    using System;

    using ComicTracker.Services.Data.Issue.Models;
    using ComicTracker.Web.Controllers;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static ComicTracker.Common.CacheConstants;
    using static ComicTracker.Tests.Data.Issue.IssueSample;

    public class IssueControllerTests
    {
        [Theory]
        [InlineData(4)]
        [InlineData(2)]
        public void IndexShouldReturnViewWithCorrectModelAndData(int id)
            // Arrange
            => MyController<IssueController>
                .Instance(controller => controller.WithData(IssueWithId(id)))
                // Act
                .Calling(c => c.Index(id))
                // Assert
                .ShouldHave()
                .MemoryCache(cache => cache
                    .ContainingEntry(entry => entry
                        .WithKey(IssueDetailsCacheKey + id.ToString())
                        .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromMinutes(1))
                        .WithValueOfType<IssueDetailsServiceModel>()))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<IssueDetailsServiceModel>()
                    .Passing(m => m.Id == id));
    }
}
