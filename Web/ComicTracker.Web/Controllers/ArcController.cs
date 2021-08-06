namespace ComicTracker.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Genre.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.ViewModels.Entities;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class ArcController : BaseController
    {
        private readonly IArcDetailsService arcDetailsService;
        private readonly IGenreRetrievalService genreRetrievalService;
        private readonly IArcCreationService arcCreationService;
        private readonly IMapper mapper;
        private readonly IArcEditingInfoService arcEditingInfoService;
        private readonly IArcEditingService arcEditingService;

        public ArcController(
            IArcDetailsService arcDetailsService,
            IGenreRetrievalService genreRetrievalService,
            IArcCreationService arcCreationService,
            IMapper mapper,
            IArcEditingInfoService arcEditingInfoService,
            IArcEditingService arcEditingService)
        {
            this.arcDetailsService = arcDetailsService;
            this.genreRetrievalService = genreRetrievalService;
            this.arcCreationService = arcCreationService;
            this.mapper = mapper;
            this.arcEditingInfoService = arcEditingInfoService;
            this.arcEditingService = arcEditingService;
        }

        public IActionResult Index(int id)
        {
            var currentArc = this.arcDetailsService.GetArc(id, this.User.GetId());

            if (currentArc == null)
            {
                return this.NotFound(currentArc);
            }

            return this.View(currentArc);
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
                var id = this.arcCreationService.CreateArc(serviceModel);

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

        [Authorize]
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
                var id = this.arcEditingService.EditArc(serviceModel);

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
    }
}
