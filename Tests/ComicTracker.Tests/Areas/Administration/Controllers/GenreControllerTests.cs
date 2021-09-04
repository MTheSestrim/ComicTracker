namespace ComicTracker.Tests.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using ComicTracker.Data.Models.Entities;
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

        [Theory]
        [InlineData("Horror")]
        [InlineData("Superhero2")]
        public void CreatePOSTShouldCreateGenreAndRedirectToIndex(string name)
            // Arrange
            => MyController<GenreController>
            // Act
            .Calling(c => c.Create(name))
            //Assert
            .ShouldHave()
            .Data(d => d.WithSet<Genre>(x => x.Any(g => g.Name == name && g.Id == 1)))
            .AndAlso()
            .ShouldReturn()
            .Redirect("/Genre");

        [Fact]
        public void CreatePOSTShouldReturnBadRequestIfGenreWithSameNameExistsInDatabase()
            // Arrange
            => MyController<GenreController>
            .Instance(controller => controller.WithData(TenGenresWithIds))
            // Act
            .Calling(c => c.Create("Genre1"))
            //Assert
            .ShouldReturn()
            .BadRequest();

        [Fact]
        public void DeleteShouldBeRestrictedForPOSTRequest()
            // Arrange
            => MyController<GenreController>
                // Act
                .Calling(c => c.Delete(2))
                // Assert
                .ShouldHave()
                .ActionAttributes(a => a.RestrictingForHttpMethod(HttpMethod.Post));

        [Theory]
        [InlineData(2)]
        [InlineData(5)]
        public void DeleteShouldDeleteGivenGenreAndRedirectToIndex(int id)
            // Arrange
            => MyController<GenreController>
                .Instance(controller => controller.WithData(TenGenresWithIds))
                // Act
                .Calling(c => c.Delete(id))
                // Assert
                .ShouldHave()
                .Data(d =>
                    d.WithSet<Genre>(x => !x.Any(g => g.Name == $"Genre{id}" && g.Id == id)))
                .AndAlso()
                .ShouldReturn()
                .Redirect($"/Genre");

        [Fact]
        public void DeleteShouldReturnNotFoundIfGenreDoesNotExist()
            // Arrange
            => MyController<ArcController>
                // Act
                .Calling(c => c.Delete(2))
                // Assert
                .ShouldReturn()
                .NotFound();
    }
}
