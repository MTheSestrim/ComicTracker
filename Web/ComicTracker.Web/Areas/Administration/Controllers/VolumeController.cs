namespace ComicTracker.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using ComicTracker.Services.Data.Genre.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Volume.Contracts;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.ViewModels.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class VolumeController : AdministrationController
    {
        private readonly IMapper mapper;
        private readonly IGenreRetrievalService genreRetrievalService;
        private readonly IVolumeCreationService volumeCreationService;
        private readonly IVolumeDeletionService volumeDeletionService;
        private readonly IVolumeEditingInfoService volumeEditingInfoService;
        private readonly IVolumeEditingService volumeEditingService;

        public VolumeController(
            IMapper mapper,
            IGenreRetrievalService genreRetrievalService,
            IVolumeCreationService volumeCreationService,
            IVolumeDeletionService volumeDeletionService,
            IVolumeEditingInfoService volumeEditingInfoService,
            IVolumeEditingService volumeEditingService)
        {
            this.mapper = mapper;
            this.genreRetrievalService = genreRetrievalService;
            this.volumeCreationService = volumeCreationService;
            this.volumeDeletionService = volumeDeletionService;
            this.volumeEditingInfoService = volumeEditingInfoService;
            this.volumeEditingService = volumeEditingService;
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

                return this.Redirect($"/Volume/{id}");
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
                CoverImage = await model.CoverImage.GetBytes(),
                CoverPath = model.CoverPath,
                Genres = model.Genres,
            };

            try
            {
                var id = this.volumeEditingService.EditVolume(serviceModel);

                return this.Redirect($"/Volume/{id}");
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
            var result = this.volumeDeletionService.DeleteVolume(id);

            if (result == -1)
            {
                return this.RedirectToAction($"/Volume/{id}");
            }

            return this.Redirect($"/Series/{result}");
        }
    }
}
