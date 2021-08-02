namespace ComicTracker.Web.Controllers.Api
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Web.Infrastructure;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [ApiController]
    [Route("api/Series")]
    public class SeriesApiController : ControllerBase
    {
        private readonly ISeriesRatingService seriesRatingService;

        public SeriesApiController(ISeriesRatingService seriesRatingService)
        {
            this.seriesRatingService = seriesRatingService;
        }

        [HttpPut]
        public async Task<ActionResult<int>> ScoreSeries(RateApiRequestModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return await this.seriesRatingService.RateSeries(this.User.GetId(), model.Id, model.Score);
            }

            return this.Unauthorized();
        }
    }
}
