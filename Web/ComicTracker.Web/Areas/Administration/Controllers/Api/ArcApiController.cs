namespace ComicTracker.Web.Areas.Administration.Controllers.Api
{
    using System;
    using System.Threading.Tasks;

    using ComicTracker.Services.Contracts;
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Infrastructure.IMemoryCacheExtensions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    [Authorize]
    [ApiController]
    [Route("api/Arc")]
    public class ArcApiController : AdministrationController
    {
        private readonly IArcTemplateCreationService arcTemplateCreationService;
        private readonly IArcAttachmentService arcAttachmentService;
        private readonly IArcDetachmentService arcDetachmentService;
        private readonly IMemoryCache cache;
        private readonly ICacheKeyHolderService<int> cacheKeyHolder;

        public ArcApiController(
            IArcTemplateCreationService arcTemplateCreationService,
            IArcAttachmentService arcAttachmentService,
            IArcDetachmentService arcDetachmentService,
            IMemoryCache cache,
            ICacheKeyHolderService<int> cacheKeyHolder)
        {
            this.arcTemplateCreationService = arcTemplateCreationService;
            this.arcAttachmentService = arcAttachmentService;
            this.arcDetachmentService = arcDetachmentService;
            this.cache = cache;
            this.cacheKeyHolder = cacheKeyHolder;
        }

        [HttpPost]
        [Route("CreateTemplates")]
        public ActionResult<int> CreateArcs(TemplateCreateApiRequestModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var numberOfArcsCreated = this.arcTemplateCreationService.CreateTemplateArcs(model);

            if (numberOfArcsCreated == null)
            {
                return this.BadRequest();
            }

            this.cache.RemoveSeriesDetails(model.SeriesId);

            return numberOfArcsCreated;
        }

        [HttpPost]
        [Route("Attach")]
        public async Task<ActionResult<int>> AttachArcsToVolume(AttachSRERequestModel model)
        {
            if (!this.ModelState.IsValid || model.MinRange > model.MaxRange)
            {
                return this.BadRequest();
            }

            try
            {
                var result = await this.arcAttachmentService.AttachArcs(model);

                this.cache.RemoveVolumeDetails(model.ParentId);
                this.cache.RemoveAllArcDetails(this.cacheKeyHolder);

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
        public async Task<ActionResult<int>> DetachArcsFromVolume(AttachSRERequestModel model)
        {
            if (!this.ModelState.IsValid || model.MinRange > model.MaxRange)
            {
                return this.BadRequest();
            }

            try
            {
                var result = await this.arcDetachmentService.DetachArcs(model);

                this.cache.RemoveVolumeDetails(model.ParentId);
                this.cache.RemoveAllArcDetails(this.cacheKeyHolder);

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
