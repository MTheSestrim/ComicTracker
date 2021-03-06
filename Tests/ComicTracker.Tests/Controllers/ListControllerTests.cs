namespace ComicTracker.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Services.Data.List.Models;
    using ComicTracker.Web.Controllers;

    using MyTested.AspNetCore.Mvc;
    
    using Xunit;

    using static ComicTracker.Tests.Data.Series.SeriesSample;

    public class ListControllerTests
    {
        [Fact]
        public void IndexShouldHaveAuthorizedAttribute()
            // Arrange
            => MyController<ListController>
                .Calling(c => c.Index())
                // Assert
                .ShouldHave()
                .ActionAttributes(attr => attr.RestrictingForAuthorizedRequests());

        [Fact]
        public void IndexShouldReturnViewWithCorrectModelandData()
            // Arrange
            => MyController<ListController>
                .Instance(controller => controller
                    .WithData(FiftySeries)
                    .WithUser(u => u.WithIdentifier("1")))
                // Act
                .Calling(c => c.Index())
                // Assert
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<IEnumerable<ListServiceModel>>()
                    .Passing(m => m.Count() == 10));
    }
}
