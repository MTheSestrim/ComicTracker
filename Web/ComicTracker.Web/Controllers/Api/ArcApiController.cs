namespace ComicTracker.Web.Controllers.Api
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.Infrastructure.IMemoryCacheExtensions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    [Authorize]
    [ApiController]
    [Route("api/Arc")]
    public class ArcApiController : ControllerBase
    {
        private readonly IArcRatingService arcRatingService;
        private readonly IMemoryCache cache;

        public ArcApiController(IArcRatingService arcRatingService, IMemoryCache cache)
        {
            this.arcRatingService = arcRatingService;
            this.cache = cache;
        }

        [HttpPut]
        public async Task<ActionResult<int>> ScoreArc(RateApiRequestModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            this.cache.RemoveArcDetails(model.Id);

            return await this.arcRatingService.RateArc(this.User.GetId(), model);
        }
    }
}
