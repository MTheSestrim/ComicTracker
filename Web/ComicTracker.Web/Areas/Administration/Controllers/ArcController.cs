namespace ComicTracker.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using ComicTracker.Services.Contracts;
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Genre.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.Infrastructure.IMemoryCacheExtensions;
    using ComicTracker.Web.ViewModels.Entities;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class ArcController : AdministrationController
    {
        private readonly IMapper mapper;
        private readonly IGenreRetrievalService genreRetrievalService;
        private readonly IArcCreationService arcCreationService;
        private readonly IArcDeletionService arcDeletionService;
        private readonly IArcEditingInfoService arcEditingInfoService;
        private readonly IArcEditingService arcEditingService;
        private readonly IMemoryCache cache;
        private readonly ICacheKeyHolderService<int> cacheKeyHolder;

        public ArcController(
            IMapper mapper,
            IGenreRetrievalService genreRetrievalService,
            IArcCreationService arcCreationService,
            IArcDeletionService arcDeletionService,
            IArcEditingInfoService arcEditingInfoService,
            IArcEditingService arcEditingService,
            IMemoryCache cache,
            ICacheKeyHolderService<int> cacheKeyHolder)
        {
            this.mapper = mapper;
            this.genreRetrievalService = genreRetrievalService;
            this.arcCreationService = arcCreationService;
            this.arcDeletionService = arcDeletionService;
            this.arcEditingInfoService = arcEditingInfoService;
            this.arcEditingService = arcEditingService;
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
                var id = this.arcCreationService.CreateArc(serviceModel);

                this.cache.RemoveSeriesDetails(model.SeriesId);

                return this.Redirect($"/Arc/{id}");
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        public IActionResult Edit(int id)
        {
            var currentArc = this.arcEditingInfoService.GetArc(id);

            if (currentArc == null)
            {
                return this.NotFound(currentArc);
            }

            var viewModel = this.mapper.Map<EditSeriesRelatedEntityInputModel>(currentArc);
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
                CoverImage = await model.CoverImage.GetBytes(),
                CoverPath = model.CoverPath,
                Genres = model.Genres,
            };

            try
            {
                var id = this.arcEditingService.EditArc(serviceModel);

                this.cache.RemoveArcDetails(id);
                this.cache.RemoveAllIssueDetails(this.cacheKeyHolder);
                this.cache.RemoveAllVolumeDetails(this.cacheKeyHolder);
                this.cache.RemoveSeriesDetails(model.SeriesId);

                return this.Redirect($"/Arc/{id}");
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var result = this.arcDeletionService.DeleteArc(id);

            if (result == -1)
            {
                return this.RedirectToAction($"/Arc/{id}");
            }

            this.cache.RemoveAllIssueDetails(this.cacheKeyHolder);
            this.cache.RemoveAllVolumeDetails(this.cacheKeyHolder);
            this.cache.RemoveSeriesDetails(result);

            return this.Redirect($"/Series/{result}");
        }
    }
}
