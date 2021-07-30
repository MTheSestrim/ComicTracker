namespace ComicTracker.Web.Controllers.Api
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.ViewModels.Entities;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/Issue")]
    public class IssueApiController : ControllerBase
    {
        private readonly IIssueRatingService issueRatingService;

        public IssueApiController(IIssueRatingService issueRatingService)
        {
            this.issueRatingService = issueRatingService;
        }

        [HttpPut]
        public async Task<ActionResult<int>> ScoreSeries(RateApiRequestModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return await this.issueRatingService.RateIssue(this.User.GetId(), model.Id, model.Score);
            }

            return this.Unauthorized();
        }
    }
}
