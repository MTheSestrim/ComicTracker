﻿namespace ComicTracker.Web.Controllers
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Web.ViewModels.Series;

    using Microsoft.AspNetCore.Mvc;

    public class SeriesController : BaseController
    {
        private readonly ISeriesDetailsService seriesDetailsService;
        private readonly IGenreRetrievalService genreRetrievalService;
        private readonly ISeriesCreationService seriesCreationService;

        public SeriesController(
            ISeriesDetailsService seriesDetailsService,
            IGenreRetrievalService genreRetrievalService,
            ISeriesCreationService seriesCreationService)
        {
            this.seriesDetailsService = seriesDetailsService;
            this.genreRetrievalService = genreRetrievalService;
            this.seriesCreationService = seriesCreationService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var currentSeries = await this.seriesDetailsService.GetSeriesAsync(id);

            if (currentSeries == null)
            {
                return this.NotFound(currentSeries);
            }

            return this.View(currentSeries);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateSeriesInputModel();
            viewModel.RetrievedGenres = await this.genreRetrievalService.GetAllAsKeyValuePairsAsync();

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSeriesInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Invalid series data.");
            }

            var id = await this.seriesCreationService.CreateSeriesAsync(model);

            return this.Redirect($"/Series/{id}");
        }
    }
}
