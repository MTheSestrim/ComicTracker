namespace ComicTracker.Web.Controllers
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Services.Data.Models.Series;
    using ComicTracker.Web.ViewModels.Series;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class SeriesController : BaseController
    {
        private readonly ISeriesDetailsService seriesDetailsService;
        private readonly IGenreRetrievalService genreRetrievalService;
        private readonly ISeriesCreationService seriesCreationService;
        private readonly ISeriesEditingInfoService seriesEditingInfoService;
        private readonly ISeriesDeletionService seriesDeletionService;

        public SeriesController(
            ISeriesDetailsService seriesDetailsService,
            IGenreRetrievalService genreRetrievalService,
            ISeriesCreationService seriesCreationService,
            ISeriesEditingInfoService seriesEditingInfoService,
            ISeriesDeletionService seriesDeletionService)
        {
            this.seriesDetailsService = seriesDetailsService;
            this.genreRetrievalService = genreRetrievalService;
            this.seriesCreationService = seriesCreationService;
            this.seriesEditingInfoService = seriesEditingInfoService;
            this.seriesDeletionService = seriesDeletionService;
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

            var serviceModel = new CreateSeriesServiceModel
            {
                Name = model.Name,
                CoverImage = model.CoverImage,
                CoverPath = model.CoverPath,
                Description = model.Description,
                Genres = model.Genres,
                Ongoing = model.Ongoing,
                RetrievedGenres = model.RetrievedGenres,
            };

            var id = await this.seriesCreationService.CreateSeriesAsync(serviceModel);

            return this.Redirect($"/Series/{id}");
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var currentSeries = this.seriesEditingInfoService.GetSeries(id);

            if (currentSeries == null)
            {
                return this.NotFound(currentSeries);
            }

            var viewModel = new EditSeriesInputModel
            {
                Id = currentSeries.Id,
                Title = currentSeries.Title,
                Ongoing = currentSeries.Ongoing,
                Description = currentSeries.Description,
                Genres = currentSeries.Genres,
                RetrievedGenres = this.genreRetrievalService.GetAllAsKeyValuePairs(),
            };

            return this.View(viewModel);
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
