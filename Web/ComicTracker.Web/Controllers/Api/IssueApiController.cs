namespace ComicTracker.Web.Controllers.Api
{
    using System;
    using System.Threading.Tasks;

    using ComicTracker.Services.Contracts;
    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.Infrastructure.IMemoryCacheExtensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    [Authorize]
    [ApiController]
    [Route("api/Issue")]
    public class IssueApiController : ControllerBase
    {
        private readonly IIssueRatingService issueRatingService;
        private readonly IIssueTemplateCreationService issueTemplateCreationService;
        private readonly IIssueAttachmentService issueAttachmentService;
        private readonly IMemoryCache cache;
        private readonly ICacheKeyHolderService<int> cacheKeyHolder;

        public IssueApiController(
            IIssueRatingService issueRatingService,
            IIssueTemplateCreationService issueTemplateCreationService,
            IIssueAttachmentService issueAttachmentService,
            IMemoryCache cache,
            ICacheKeyHolderService<int> cacheKeyHolder)
        {
            this.issueRatingService = issueRatingService;
            this.issueTemplateCreationService = issueTemplateCreationService;
            this.issueAttachmentService = issueAttachmentService;
            this.cache = cache;
            this.cacheKeyHolder = cacheKeyHolder;
        }

        [HttpPut]
        public async Task<ActionResult<int>> ScoreIssue(RateApiRequestModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            this.cache.RemoveIssueDetails(model.Id);

            return await this.issueRatingService.RateIssue(this.User.GetId(), model);
        }

        [HttpPost]
        [Route("CreateTemplates")]
        public ActionResult<int> CreateIssues(TemplateCreateApiRequestModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var numberOfIssuesCreated = this.issueTemplateCreationService.CreateTemplateIssues(model);

            if (numberOfIssuesCreated == null)
            {
                return this.BadRequest();
            }

            this.cache.RemoveSeriesDetails(model.SeriesId);

            return numberOfIssuesCreated;
        }

        [HttpPost]
        [Route("Attach")]
        public async Task<ActionResult<int>> AttachIssuesToSeriesRelatedEntity(AttachSRERequestModel model)
        {
            if (!this.ModelState.IsValid || model.MinRange > model.MaxRange)
            {
                return this.BadRequest();
            }

            try
            {
                var result = await this.issueAttachmentService.AttachIssues(model);

                if (model.ParentTypeName == "Arc")
                {
                    this.cache.RemoveArcDetails(model.ParentId);
                }
                else if (model.ParentTypeName == "Volume")
                {
                    this.cache.RemoveVolumeDetails(model.ParentId);
                }

                this.cache.RemoveAllIssueDetails(this.cacheKeyHolder);

                return result;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
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
