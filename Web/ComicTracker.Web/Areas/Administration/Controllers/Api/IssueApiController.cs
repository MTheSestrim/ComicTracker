namespace ComicTracker.Web.Areas.Administration.Controllers.Api
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
    public class IssueApiController : AdministrationController
    {
        private readonly IIssueTemplateCreationService issueTemplateCreationService;
        private readonly IIssueAttachmentService issueAttachmentService;
        private readonly IIssueDetachmentService issueDetachmentService;
        private readonly IMemoryCache cache;
        private readonly ICacheKeyHolderService<int> cacheKeyHolder;

        public IssueApiController(
            IIssueTemplateCreationService issueTemplateCreationService,
            IIssueAttachmentService issueAttachmentService,
            IIssueDetachmentService issueDetachmentService,
            IMemoryCache cache,
            ICacheKeyHolderService<int> cacheKeyHolder)
        {
            this.issueTemplateCreationService = issueTemplateCreationService;
            this.issueAttachmentService = issueAttachmentService;
            this.issueDetachmentService = issueDetachmentService;
            this.cache = cache;
            this.cacheKeyHolder = cacheKeyHolder;
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
                return this.BadRequest("Invalid model.");
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
            catch (ArgumentOutOfRangeException)
            {
                return this.BadRequest("Incorrect issue range given or series given.");
            }
            catch (ArgumentNullException)
            {
                return this.NotFound($"{model.ParentTypeName} with given id {model.ParentId} does not exist.");
            }
        }

        [HttpDelete]
        public async Task<ActionResult<int>> DetachIssuesFromSeriesRelatedEntity(AttachSRERequestModel model)
        {
            if (!this.ModelState.IsValid || model.MinRange > model.MaxRange)
            {
                return this.BadRequest("Invalid model.");
            }

            try
            {
                var result = await this.issueDetachmentService.DetachIssues(model);

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
            catch (ArgumentOutOfRangeException)
            {
                return this.BadRequest("Incorrect issue range or series given.");
            }
            catch (ArgumentNullException)
            {
                return this.NotFound($"{model.ParentTypeName} with given id {model.ParentId} does not exist.");
            }
        }
    }
}
