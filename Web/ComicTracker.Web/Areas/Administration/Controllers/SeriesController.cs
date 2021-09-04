namespace ComicTracker.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using ComicTracker.Services.Contracts;
    using ComicTracker.Services.Data.Genre.Contracts;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Services.Data.Series.Models;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.Infrastructure.IMemoryCacheExtensions;
    using ComicTracker.Web.ViewModels.Series;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class SeriesController : AdministrationController
    {
        private readonly IMapper mapper;
        private readonly IGenreRetrievalService genreRetrievalService;
        private readonly ISeriesCreationService seriesCreationService;
        private readonly ISeriesDeletionService seriesDeletionService;
        private readonly ISeriesEditingInfoService seriesEditingInfoService;
        private readonly ISeriesEditingService seriesEditingService;
        private readonly IMemoryCache cache;
        private readonly ICacheKeyHolderService<int> cacheKeyHolder;

        public SeriesController(
            IMapper mapper,
            IGenreRetrievalService genreRetrievalService,
            ISeriesCreationService seriesCreationService,
            ISeriesDeletionService seriesDeletionService,
            ISeriesEditingInfoService seriesEditingInfoService,
            ISeriesEditingService seriesEditingService,
            IMemoryCache cache,
            ICacheKeyHolderService<int> cacheKeyHolder)
        {
            this.mapper = mapper;
            this.genreRetrievalService = genreRetrievalService;
            this.seriesCreationService = seriesCreationService;
            this.seriesDeletionService = seriesDeletionService;
            this.seriesEditingInfoService = seriesEditingInfoService;
            this.seriesEditingService = seriesEditingService;
            this.cache = cache;
            this.cacheKeyHolder = cacheKeyHolder;
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
                return this.NotFound();
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

            if (id == null)
            {
                return this.NotFound();
            }

            this.cache.RemoveSeriesDetails(id.Value);
            this.cache.RemoveAllArcDetails(this.cacheKeyHolder);
            this.cache.RemoveAllVolumeDetails(this.cacheKeyHolder);
            this.cache.RemoveAllIssueDetails(this.cacheKeyHolder);

            return this.Redirect($"/Series/{id.Value}");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result = this.seriesDeletionService.DeleteSeries(id);

            if (!result)
            {
                return this.NotFound();
            }

            this.cache.RemoveAllArcDetails(this.cacheKeyHolder);
            this.cache.RemoveAllIssueDetails(this.cacheKeyHolder);
            this.cache.RemoveAllVolumeDetails(this.cacheKeyHolder);
            this.cache.RemoveSeriesDetails(id);

            return this.Redirect("/");
        }
    }
}
