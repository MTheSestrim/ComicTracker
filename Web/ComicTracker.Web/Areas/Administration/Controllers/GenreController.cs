namespace ComicTracker.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Genre.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class GenreController : AdministrationController
    {
        private readonly IGenreRetrievalService genreRetrievalService;
        private readonly IGenreCreationService genreCreationService;
        private readonly IGenreDeletionService genreDeletionService;

        public GenreController(
            IGenreRetrievalService genreRetrievalService,
            IGenreCreationService genreCreationService,
            IGenreDeletionService genreDeletionService)
        {
            this.genreRetrievalService = genreRetrievalService;
            this.genreCreationService = genreCreationService;
            this.genreDeletionService = genreDeletionService;
        }

        public IActionResult Index()
        {
            var genres = this.genreRetrievalService.GetAllAsKeyValuePairs();

            return this.View(genres);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var result = await this.genreCreationService.CreateGenre(name);

            if (!result)
            {
                return this.BadRequest();
            }

            return this.Redirect("/Genre");
        }

        [HttpPost]

        public async Task<IActionResult> Delete(int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Genre");
            }

            var result = await this.genreDeletionService.DeleteGenre(id);

            if (!result)
            {
                return this.NotFound();
            }

            return this.Redirect("/Genre");
        }
    }
}
