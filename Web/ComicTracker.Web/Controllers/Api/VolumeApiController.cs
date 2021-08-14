namespace ComicTracker.Web.Controllers.Api
{
    using System;
    using System.Threading.Tasks;
    using ComicTracker.Services.Contracts;
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
        private readonly IVolumeAttachmentService volumeAttachmentService;
        private readonly IMemoryCache cache;
        private readonly Services.Contracts.ICacheKeyHolderService<int> cacheKeyHolder;

        public VolumeApiController(
            IVolumeRatingService volumeRatingService,
            IVolumeTemplateCreationService volumeTemplateCreationService,
            IVolumeAttachmentService volumeAttachmentService,
            IMemoryCache cache,
            ICacheKeyHolderService<int> cacheKeyHolder)
        {
            this.volumeRatingService = volumeRatingService;
            this.volumeTemplateCreationService = volumeTemplateCreationService;
            this.volumeAttachmentService = volumeAttachmentService;
            this.cache = cache;
            this.cacheKeyHolder = cacheKeyHolder;
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
        [Route("CreateTemplates")]
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

        [HttpPost]
        [Route("Attach")]
        public async Task<ActionResult<int>> AttachVolumesToArc(AttachSRERequestModel model)
        {
            if (!this.ModelState.IsValid || model.MinRange > model.MaxRange)
            {
                return this.BadRequest();
            }

            try
            {
                var result = await this.volumeAttachmentService.AttachVolumes(model);

                this.cache.RemoveArcDetails(model.ParentId);
                this.cache.RemoveAllVolumeDetails(this.cacheKeyHolder);

                return result;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}
