namespace ComicTracker.Web.Controllers.Api
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Volume.Contracts;
    using ComicTracker.Web.Infrastructure;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
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
        public async Task<ActionResult<int>> ScoreVolume(RateApiRequestModel model)
            => await this.volumeRatingService.RateVolume(this.User.GetId(), model.Id, model.Score);
    }
}
