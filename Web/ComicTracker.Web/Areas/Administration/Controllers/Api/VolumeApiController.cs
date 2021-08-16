namespace ComicTracker.Web.Areas.Administration.Controllers.Api
{
    using System;
    using System.Threading.Tasks;

    using ComicTracker.Services.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Volume.Contracts;
    using ComicTracker.Web.Infrastructure.IMemoryCacheExtensions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    [Authorize]
    [ApiController]
    [Route("api/Volume")]
    public class VolumeApiController : AdministrationController
    {
        private readonly IVolumeTemplateCreationService volumeTemplateCreationService;
        private readonly IVolumeAttachmentService volumeAttachmentService;
        private readonly IVolumeDetachmentService volumeDetachmentService;
        private readonly IMemoryCache cache;
        private readonly ICacheKeyHolderService<int> cacheKeyHolder;

        public VolumeApiController(
            IVolumeTemplateCreationService volumeTemplateCreationService,
            IVolumeAttachmentService volumeAttachmentService,
            IVolumeDetachmentService volumeDetachmentService,
            IMemoryCache cache,
            ICacheKeyHolderService<int> cacheKeyHolder)
        {
            this.volumeTemplateCreationService = volumeTemplateCreationService;
            this.volumeAttachmentService = volumeAttachmentService;
            this.volumeDetachmentService = volumeDetachmentService;
            this.cache = cache;
            this.cacheKeyHolder = cacheKeyHolder;
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

        [HttpDelete]
        [Route("Detach")]
        public async Task<ActionResult<int>> DetachVolumesFromArc(AttachSRERequestModel model)
        {
            if (!this.ModelState.IsValid || model.MinRange > model.MaxRange)
            {
                return this.BadRequest();
            }

            try
            {
                var result = await this.volumeDetachmentService.DetachVolumes(model);

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
