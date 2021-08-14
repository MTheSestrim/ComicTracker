namespace ComicTracker.Web.Controllers.Api
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.Infrastructure.IMemoryCacheExtensions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    [Authorize]
    [ApiController]
    [Route("api/Series")]
    public class SeriesApiController : ControllerBase
    {
        private readonly ISeriesRatingService seriesRatingService;
        private readonly IMemoryCache cache;

        public SeriesApiController(ISeriesRatingService seriesRatingService, IMemoryCache cache)
        {
            this.seriesRatingService = seriesRatingService;
            this.cache = cache;
        }

        [HttpPut]
        public async Task<ActionResult<int?>> ScoreSeries(RateApiRequestModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            this.cache.RemoveSeriesDetails(model.Id);

            return await this.seriesRatingService.RateSeries(this.User.GetId(), model);
        }
    }
}
