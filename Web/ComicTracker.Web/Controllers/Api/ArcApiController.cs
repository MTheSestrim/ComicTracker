namespace ComicTracker.Web.Controllers.Api
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Infrastructure;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/Arc")]
    public class ArcApiController : ControllerBase
    {
        private readonly IArcRatingService arcRatingService;

        public ArcApiController(IArcRatingService arcRatingService)
        {
            this.arcRatingService = arcRatingService;
        }

        [HttpPut]
        public async Task<ActionResult<int>> ScoreSeries(RateApiRequestModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return await this.arcRatingService.RateArc(this.User.GetId(), model.Id, model.Score);
            }

            return this.Unauthorized();
        }
    }
}
