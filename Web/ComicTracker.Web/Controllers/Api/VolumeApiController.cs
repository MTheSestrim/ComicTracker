namespace ComicTracker.Web.Controllers.Api
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.ViewModels.Entities;

    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/Volume")]
    public class VolumeApiController : ControllerBase
    {
        private readonly IVolumeRatingService volumeRatingService;

        public VolumeApiController(IVolumeRatingService volumeRatingService)
        {
            this.volumeRatingService = volumeRatingService;
        }

        [HttpPut]
        public async Task<ActionResult<int>> ScoreSeries(RateApiRequestModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return await this.volumeRatingService.RateVolume(this.User.GetId(), model.Id, model.Score);
            }

            return this.Unauthorized();
        }
    }
}
