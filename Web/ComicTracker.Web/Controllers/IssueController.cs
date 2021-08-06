﻿namespace ComicTracker.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using ComicTracker.Services.Data.Genre.Contracts;
    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.ViewModels.Entities;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class IssueController : BaseController
    {
        private readonly IIssueDetailsService issueDetailsService;
        private readonly IGenreRetrievalService genreRetrievalService;
        private readonly IIssueCreationService issueCreationService;
        private readonly IMapper mapper;
        private readonly IIssueEditingInfoService issueEditingInfoService;
        private readonly IIssueEditingService issueEditingService;

        public IssueController(
            IIssueDetailsService issueDetailsService,
            IGenreRetrievalService genreRetrievalService,
            IIssueCreationService issueCreationService,
            IMapper mapper,
            IIssueEditingInfoService issueEditingInfoService,
            IIssueEditingService issueEditingService)
        {
            this.issueDetailsService = issueDetailsService;
            this.genreRetrievalService = genreRetrievalService;
            this.issueCreationService = issueCreationService;
            this.mapper = mapper;
            this.issueEditingInfoService = issueEditingInfoService;
            this.issueEditingService = issueEditingService;
        }

        public IActionResult Index(int id)
        {
            var currentIssue = this.issueDetailsService.GetIssue(id, this.User.GetId());

            if (currentIssue == null)
            {
                return this.NotFound(currentIssue);
            }

            return this.View(currentIssue);
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
                var id = this.issueCreationService.CreateIssue(serviceModel);

                return this.Redirect($"/Issue/{id}");
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
            var currentIssue = this.issueEditingInfoService.GetIssue(id);

            if (currentIssue == null)
            {
                return this.NotFound(currentIssue);
            }

            var viewModel = this.mapper.Map<EditSeriesRelatedEntityInputModel>(currentIssue);
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
                var id = this.issueEditingService.EditIssue(serviceModel);

                return this.Redirect($"/Issue/{id}");
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
