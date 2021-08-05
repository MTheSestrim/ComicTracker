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
        private readonly IVolumeTemplateCreationService volumeTemplateCreationService;

        public VolumeApiController(
            IVolumeRatingService volumeRatingService,
            IVolumeTemplateCreationService volumeTemplateCreationService)
        {
            this.volumeRatingService = volumeRatingService;
            this.volumeTemplateCreationService = volumeTemplateCreationService;
        }

        [HttpPut]
        public async Task<ActionResult<int>> ScoreVolume(RateApiRequestModel model)
            => await this.volumeRatingService.RateVolume(this.User.GetId(), model);

        [HttpPost]
        public ActionResult<int> CreateIssues(TemplateCreateApiRequestModel model)
        {
            return this.volumeTemplateCreationService.CreateTemplateVolumes(model);
        }
    }
}
