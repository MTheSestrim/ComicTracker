namespace ComicTracker.Web.Controllers.Api
{
    using System.Threading.Tasks;

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
        private readonly IMemoryCache cache;

        public IssueApiController(
            IIssueRatingService issueRatingService,
            IIssueTemplateCreationService issueTemplateCreationService,
            IMemoryCache cache)
        {
            this.issueRatingService = issueRatingService;
            this.issueTemplateCreationService = issueTemplateCreationService;
            this.cache = cache;
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
    }
}
