namespace ComicTracker.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using ComicTracker.Services.Contracts;
    using ComicTracker.Services.Data.Genre.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Volume.Contracts;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.Infrastructure.IMemoryCacheExtensions;
    using ComicTracker.Web.ViewModels.Entities;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class VolumeController : AdministrationController
    {
        private readonly IMapper mapper;
        private readonly IGenreRetrievalService genreRetrievalService;
        private readonly IVolumeCreationService volumeCreationService;
        private readonly IVolumeDeletionService volumeDeletionService;
        private readonly IVolumeEditingInfoService volumeEditingInfoService;
        private readonly IVolumeEditingService volumeEditingService;
        private readonly IMemoryCache cache;
        private readonly ICacheKeyHolderService<int> cacheKeyHolder;

        public VolumeController(
            IMapper mapper,
            IGenreRetrievalService genreRetrievalService,
            IVolumeCreationService volumeCreationService,
            IVolumeDeletionService volumeDeletionService,
            IVolumeEditingInfoService volumeEditingInfoService,
            IVolumeEditingService volumeEditingService,
            IMemoryCache cache,
            ICacheKeyHolderService<int> cacheKeyHolder)
        {
            this.mapper = mapper;
            this.genreRetrievalService = genreRetrievalService;
            this.volumeCreationService = volumeCreationService;
            this.volumeDeletionService = volumeDeletionService;
            this.volumeEditingInfoService = volumeEditingInfoService;
            this.volumeEditingService = volumeEditingService;
            this.cache = cache;
            this.cacheKeyHolder = cacheKeyHolder;
        }

        public IActionResult Create(int id, int number = 0)
        {
            var viewModel = new CreateSeriesRelatedEntityInputModel
            {
                RetrievedGenres = this.genreRetrievalService.GetAllAsKeyValuePairs(),
                SeriesId = id,
                Number = number,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSeriesRelatedEntityInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.RetrievedGenres = this.genreRetrievalService.GetAllAsKeyValuePairs();
                return this.View(model);
            }

            var serviceModel = new CreateSeriesRelatedEntityServiceModel
            {
                Title = model.Title,
                Number = model.Number,
                CoverImage = await model.CoverImage.GetBytes(),
                CoverPath = model.CoverPath,
                Description = model.Description,
                Genres = model.Genres,
                SeriesId = model.SeriesId,
                RetrievedGenres = model.RetrievedGenres,
            };

            try
            {
                var id = this.volumeCreationService.CreateVolume(serviceModel);

                this.cache.RemoveSeriesDetails(model.SeriesId);

                return this.Redirect($"/Volume/{id}");
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound();
            }
            catch (InvalidOperationException)
            {
                return this.BadRequest();
            }
        }

        public IActionResult Edit(int id)
        {
            var currentVolume = this.volumeEditingInfoService.GetVolume(id);

            if (currentVolume == null)
            {
                return this.NotFound(currentVolume);
            }

            var viewModel = this.mapper.Map<EditSeriesRelatedEntityInputModel>(currentVolume);
            viewModel.RetrievedGenres = this.genreRetrievalService.GetAllAsKeyValuePairs();

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSeriesRelatedEntityInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.RetrievedGenres = this.genreRetrievalService.GetAllAsKeyValuePairs();
                return this.View(model);
            }

            var serviceModel = new EditSeriesRelatedEntityServiceModel
            {
                Id = model.Id,
                Title = model.Title,
                Number = model.Number,
                Description = model.Description,
                SeriesId = model.SeriesId,
                CoverImage = await model.CoverImage.GetBytes(),
                CoverPath = model.CoverPath,
                Genres = model.Genres,
            };

            try
            {
                var id = this.volumeEditingService.EditVolume(serviceModel);

                this.cache.RemoveVolumeDetails(id);
                this.cache.RemoveAllIssueDetails(this.cacheKeyHolder);
                this.cache.RemoveAllArcDetails(this.cacheKeyHolder);
                this.cache.RemoveSeriesDetails(model.SeriesId);

                return this.Redirect($"/Volume/{id}");
            }
            catch (KeyNotFoundException)
            {
                return this.NotFound();
            }
            catch (InvalidOperationException)
            {
                return this.BadRequest();
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result = this.volumeDeletionService.DeleteVolume(id);

            if (result == null)
            {
                return this.NotFound(result);
            }

            this.cache.RemoveSeriesDetails(result.Value);
            this.cache.RemoveAllIssueDetails(this.cacheKeyHolder);
            this.cache.RemoveAllArcDetails(this.cacheKeyHolder);
            this.cache.RemoveVolumeDetails(id);

            return this.Redirect($"/Series/{result.Value}");
        }
    }
}
