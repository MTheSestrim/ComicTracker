namespace ComicTracker.Web.Controllers.Api
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Volume.Contracts;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.Infrastructure.IMemoryCacheExtensions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    [Authorize]
    [ApiController]
    [Route("api/Volume")]
    public class VolumeApiController : ControllerBase
    {
        private readonly IVolumeRatingService volumeRatingService;
        private readonly IVolumeTemplateCreationService volumeTemplateCreationService;
        private readonly IMemoryCache cache;

        public VolumeApiController(
            IVolumeRatingService volumeRatingService,
            IVolumeTemplateCreationService volumeTemplateCreationService,
            IMemoryCache cache)
        {
            this.volumeRatingService = volumeRatingService;
            this.volumeTemplateCreationService = volumeTemplateCreationService;
            this.cache = cache;
        }

        [HttpPut]
        public async Task<ActionResult<int>> ScoreVolume(RateApiRequestModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            this.cache.RemoveVolumeDetails(model.Id);

            return await this.volumeRatingService.RateVolume(this.User.GetId(), model);
        }

        [HttpPost]
        public ActionResult<int> CreateVolumes(TemplateCreateApiRequestModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var numberOfVolumesCreated = this.volumeTemplateCreationService.CreateTemplateVolumes(model);

            if (numberOfVolumesCreated == null)
            {
                return this.BadRequest();
            }

            this.cache.RemoveSeriesDetails(model.SeriesId);

            return numberOfVolumesCreated;
        }
    }
}
