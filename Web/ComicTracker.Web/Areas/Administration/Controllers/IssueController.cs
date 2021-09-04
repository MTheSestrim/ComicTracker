namespace ComicTracker.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using ComicTracker.Services.Contracts;
    using ComicTracker.Services.Data.Genre.Contracts;
    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.Infrastructure.IMemoryCacheExtensions;
    using ComicTracker.Web.ViewModels.Entities;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class IssueController : AdministrationController
    {
        private readonly IMapper mapper;
        private readonly IGenreRetrievalService genreRetrievalService;
        private readonly IIssueCreationService issueCreationService;
        private readonly IIssueDetachmentService issueDetachmentService;
        private readonly IIssueDeletionService issueDeletionService;
        private readonly IIssueEditingInfoService issueEditingInfoService;
        private readonly IIssueEditingService issueEditingService;
        private readonly IMemoryCache cache;
        private readonly ICacheKeyHolderService<int> cacheKeyHolder;

        public IssueController(
            IMapper mapper,
            IGenreRetrievalService genreRetrievalService,
            IIssueCreationService issueCreationService,
            IIssueDetachmentService issueDetachmentService,
            IIssueDeletionService issueDeletionService,
            IIssueEditingInfoService issueEditingInfoService,
            IIssueEditingService issueEditingService,
            IMemoryCache cache,
            ICacheKeyHolderService<int> cacheKeyHolder)
        {
            this.mapper = mapper;
            this.genreRetrievalService = genreRetrievalService;
            this.issueCreationService = issueCreationService;
            this.issueDetachmentService = issueDetachmentService;
            this.issueDeletionService = issueDeletionService;
            this.issueEditingInfoService = issueEditingInfoService;
            this.issueEditingService = issueEditingService;
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
                var id = this.issueCreationService.CreateIssue(serviceModel);

                this.cache.RemoveSeriesDetails(model.SeriesId);

                return this.Redirect($"/Issue/{id}");
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
            var currentIssue = this.issueEditingInfoService.GetIssue(id);

            if (currentIssue == null)
            {
                return this.NotFound();
            }

            var viewModel = this.mapper.Map<EditSeriesRelatedEntityInputModel>(currentIssue);
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
                var id = this.issueEditingService.EditIssue(serviceModel);

                this.cache.RemoveIssueDetails(id);
                this.cache.RemoveAllArcDetails(this.cacheKeyHolder);
                this.cache.RemoveAllVolumeDetails(this.cacheKeyHolder);
                this.cache.RemoveSeriesDetails(model.SeriesId);

                return this.Redirect($"/Issue/{id}");
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
            var result = this.issueDeletionService.DeleteIssue(id);

            if (result == null)
            {
                return this.NotFound(result);
            }

            this.cache.RemoveSeriesDetails(result.Value);
            this.cache.RemoveAllArcDetails(this.cacheKeyHolder);
            this.cache.RemoveAllVolumeDetails(this.cacheKeyHolder);
            this.cache.RemoveIssueDetails(id);

            return this.Redirect($"/Series/{result.Value}");
        }

        [HttpPost]
        public IActionResult RemoveArc(int id)
        {
            var result = this.issueDetachmentService.DetachArcFromIssue(id);

            if (result.Result == null)
            {
                return this.NotFound();
            }

            this.cache.RemoveIssueDetails(result.Result.Value);
            this.cache.RemoveAllArcDetails(this.cacheKeyHolder);

            return this.Redirect($"/Issue/{result.Result.Value}");
        }

        [HttpPost]
        public IActionResult RemoveVolume(int id)
        {
            var result = this.issueDetachmentService.DetachVolumeFromIssue(id);

            if (result.Result == null)
            {
                return this.NotFound();
            }

            this.cache.RemoveIssueDetails(result.Result.Value);
            this.cache.RemoveAllVolumeDetails(this.cacheKeyHolder);

            return this.Redirect($"/Issue/{result.Result.Value}");
        }
    }
}
