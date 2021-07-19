namespace ComicTracker.Web.Controllers
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.ViewModels.Series;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class SeriesController : BaseController
    {
        private readonly ISeriesDetailsService seriesDetailsService;
        private readonly IGenreRetrievalService genreRetrievalService;
        private readonly ISeriesCreationService seriesCreationService;
        private readonly ISeriesDeletionService seriesDeletionService;
        private readonly ISeriesRatingService seriesRatingService;

        public SeriesController(
            ISeriesDetailsService seriesDetailsService,
            IGenreRetrievalService genreRetrievalService,
            ISeriesCreationService seriesCreationService,
            ISeriesDeletionService seriesDeletionService,
            ISeriesRatingService seriesRatingService)
        {
            this.seriesDetailsService = seriesDetailsService;
            this.genreRetrievalService = genreRetrievalService;
            this.seriesCreationService = seriesCreationService;
            this.seriesDeletionService = seriesDeletionService;
            this.seriesRatingService = seriesRatingService;
        }

        public IActionResult Index(int id)
        {
            var currentSeries = this.seriesDetailsService.GetSeries(id);

            if (currentSeries == null)
            {
                return this.NotFound(currentSeries);
            }

            return this.View(currentSeries);
        }

        [Authorize]
        public IActionResult Create()
        {
            var viewModel = new CreateSeriesInputModel();
            viewModel.RetrievedGenres = this.genreRetrievalService.GetAllAsKeyValuePairs();

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateSeriesInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.RetrievedGenres = this.genreRetrievalService.GetAllAsKeyValuePairs();
                return this.View(model);
            }

            var id = await this.seriesCreationService.CreateSeriesAsync(model);

            return this.Redirect($"/Series/{id}");
        }

        [HttpPost]
        [Authorize]
        public IActionResult Rate(RateSeriesInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(model);
            }

            this.seriesRatingService.RateSeries(this.User.GetId(), model.SeriesId, model.Score);

            return this.StatusCode(201);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await this.seriesDeletionService.DeleteSeries(id);

            if (!result)
            {
                return this.RedirectToAction($"/Series/{id}");
            }

            return this.Redirect("/");
        }
    }
}
