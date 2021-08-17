namespace ComicTracker.Tests.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Web.Areas.Administration.Controllers;

    using MyTested.AspNetCore.Mvc;

    using Xunit;

    using static ComicTracker.Tests.Data.Genre.GenreSample;

    public class GenreControllerTests
    {
        [Fact]
        public void IndexShouldReturnViewWithGenres()
            // Arrange
            => MyController<GenreController>
            .Instance(controller => controller.WithData(TenGenresWithIds))
            // Act
            .Calling(c => c.Index())
            //Assert
            .ShouldReturn()
            .View(view => view
                    .WithModelOfType<IEnumerable<KeyValuePair<string, string>>>()
                    .Passing(m => m.Count() == TenGenresWithIds.Count()));

        [Fact]
        public void CreateShouldReturnView()
            // Arrange
            => MyController<GenreController>
            // Act
            .Calling(c => c.Create())
            //Assert
            .ShouldReturn()
            .View();
    }
}
