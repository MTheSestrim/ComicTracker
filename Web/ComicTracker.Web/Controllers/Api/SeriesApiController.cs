namespace ComicTracker.Web.Controllers.Api
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Infrastructure;

    using Microsoft.AspNetCore.Mvc;

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
