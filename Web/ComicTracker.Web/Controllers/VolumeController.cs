﻿namespace ComicTracker.Web.Controllers
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

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class VolumeController : BaseController
    {
        private readonly IVolumeDetailsService volumeDetailsService;
        private readonly IGenreRetrievalService genreRetrievalService;
        private readonly IVolumeCreationService volumeCreationService;
        private readonly IMapper mapper;
        private readonly IVolumeEditingInfoService volumeEditingInfoService;
        private readonly IVolumeEditingService volumeEditingService;

        public VolumeController(
            IVolumeDetailsService volumeDetailsService,
            IGenreRetrievalService genreRetrievalService,
            IVolumeCreationService volumeCreationService,
            IMapper mapper,
            IVolumeEditingInfoService volumeEditingInfoService,
            IVolumeEditingService volumeEditingService)
        {
            this.volumeDetailsService = volumeDetailsService;
            this.genreRetrievalService = genreRetrievalService;
            this.volumeCreationService = volumeCreationService;
            this.mapper = mapper;
            this.volumeEditingInfoService = volumeEditingInfoService;
            this.volumeEditingService = volumeEditingService;
        }

        public IActionResult Index(int id)
        {
            var currentVolume = this.volumeDetailsService.GetVolume(id, this.User.GetId());

            if (currentVolume == null)
            {
                return this.NotFound(currentVolume);
            }

            return this.View(currentVolume);
        }

        [Authorize]
        public IActionResult Create(int id, int number = 0)
        {
            var viewModel = new CreateSeriesRelatedEntityInputModel();
            viewModel.RetrievedGenres = this.genreRetrievalService.GetAllAsKeyValuePairs();
            viewModel.SeriesId = id;
            viewModel.Number = number;

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize]
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

        [Authorize]
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
        [Authorize]
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
    }
}
