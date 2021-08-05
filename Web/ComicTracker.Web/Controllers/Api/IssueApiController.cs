namespace ComicTracker.Web.Controllers.Api
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Infrastructure;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiController]
    [Route("api/Issue")]
    public class IssueApiController : ControllerBase
    {
        private readonly IIssueRatingService issueRatingService;
        private readonly IIssueTemplateCreationService issueTemplateCreationService;

        public IssueApiController(
            IIssueRatingService issueRatingService,
            IIssueTemplateCreationService issueTemplateCreationService)
        {
            this.issueRatingService = issueRatingService;
            this.issueTemplateCreationService = issueTemplateCreationService;
        }

        [HttpPut]
        public async Task<ActionResult<int>> ScoreIssue(RateApiRequestModel model)
            => await this.issueRatingService.RateIssue(this.User.GetId(), model);

        [HttpPost]
        public ActionResult<int> CreateIssues(TemplateCreateApiRequestModel model)
        {
            return this.issueTemplateCreationService.CreateTemplateIssues(model);
        }
    }
}
