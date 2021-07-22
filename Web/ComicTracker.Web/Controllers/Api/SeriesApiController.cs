namespace ComicTracker.Web.Controllers.Api
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Web.ViewModels.Series;

    using Microsoft.AspNetCore.Mvc;

    using static ComicTracker.Web.Infrastructure.ClaimsPrincipalExtensions;

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
        public async Task<ActionResult<int>> ScoreSeries(RateSeriesInputModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return await this.seriesRatingService.RateSeries(this.User.GetId(), model.SeriesId, model.Score);
            }

            return this.Unauthorized();
        }
    }
}
