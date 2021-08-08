namespace ComicTracker.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using ComicTracker.Services.Data.Genre.Contracts;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Services.Data.Series.Models;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.ViewModels.Series;

    using Microsoft.AspNetCore.Mvc;

    public class SeriesController : AdministrationController
    {
        private readonly IMapper mapper;
        private readonly IGenreRetrievalService genreRetrievalService;
        private readonly ISeriesCreationService seriesCreationService;
        private readonly ISeriesDeletionService seriesDeletionService;
        private readonly ISeriesEditingInfoService seriesEditingInfoService;
        private readonly ISeriesEditingService seriesEditingService;

        public SeriesController(
            IMapper mapper,
            IGenreRetrievalService genreRetrievalService,
            ISeriesCreationService seriesCreationService,
            ISeriesDeletionService seriesDeletionService,
            ISeriesEditingInfoService seriesEditingInfoService,
            ISeriesEditingService seriesEditingService)
        {
            this.mapper = mapper;
            this.genreRetrievalService = genreRetrievalService;
            this.seriesCreationService = seriesCreationService;
            this.seriesDeletionService = seriesDeletionService;
            this.seriesEditingInfoService = seriesEditingInfoService;
            this.seriesEditingService = seriesEditingService;
        }

        public IActionResult Create()
        {
            var viewModel = new CreateSeriesInputModel
            {
                RetrievedGenres = this.genreRetrievalService.GetAllAsKeyValuePairs(),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSeriesInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.RetrievedGenres = this.genreRetrievalService.GetAllAsKeyValuePairs();
                return this.View(model);
            }

            var serviceModel = new CreateSeriesServiceModel
            {
                Title = model.Title,
                CoverImage = await model.CoverImage.GetBytes(),
                CoverPath = model.CoverPath,
                Description = model.Description,
                Genres = model.Genres,
                Ongoing = model.Ongoing,
                RetrievedGenres = model.RetrievedGenres,
            };

            var id = this.seriesCreationService.CreateSeries(serviceModel);

            return this.Redirect($"/Series/{id}");
        }

        public IActionResult Edit(int id)
        {
            var currentSeries = this.seriesEditingInfoService.GetSeries(id);

            if (currentSeries == null)
            {
                return this.NotFound(currentSeries);
            }

            var viewModel = this.mapper.Map<EditSeriesInputModel>(currentSeries);
            viewModel.RetrievedGenres = this.genreRetrievalService.GetAllAsKeyValuePairs();

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSeriesInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.RetrievedGenres = this.genreRetrievalService.GetAllAsKeyValuePairs();
                return this.View(model);
            }

            var serviceModel = new EditSeriesServiceModel
            {
                Id = model.Id,
                Title = model.Title,
                Ongoing = model.Ongoing,
                Description = model.Description,
                CoverImage = await model.CoverImage.GetBytes(),
                CoverPath = model.CoverPath,
                Genres = model.Genres,
            };

            var id = this.seriesEditingService.EditSeries(serviceModel);

            return this.Redirect($"/Series/{id}");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result = this.seriesDeletionService.DeleteSeries(id);

            if (!result)
            {
                return this.RedirectToAction($"/Series/{id}");
            }

            return this.Redirect("/");
        }
    }
}
