namespace ComicTracker.Web.Controllers.Api
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.List.Contracts;
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
    public class VolumeApiController : BaseController
    {
        private readonly IVolumeRatingService volumeRatingService;
        private readonly IListVolumeService listVolumeService;
        private readonly IMemoryCache cache;

        public VolumeApiController(
            IVolumeRatingService volumeRatingService,
            IListVolumeService listVolumeService,
            IMemoryCache cache)
        {
            this.volumeRatingService = volumeRatingService;
            this.listVolumeService = listVolumeService;
            this.cache = cache;
        }

        [HttpPut]
        [Route("Score")]
        public async Task<ActionResult<int>> ScoreVolume(RateApiRequestModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            this.cache.RemoveVolumeDetails(model.Id);

            return await this.volumeRatingService.RateVolume(this.User.GetId(), model);
        }

        [HttpPut]
        [Route("AddToList/{id}")]
        public async Task<ActionResult> AddVolumeToList(int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            this.cache.RemoveVolumeDetails(id);

            try
            {
                await this.listVolumeService.AddVolumeToList(this.User.GetId(), id);

                return this.NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("RemoveFromList/{id}")]
        public async Task<ActionResult> RemoveVolumeFromList(int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            this.cache.RemoveVolumeDetails(id);

            try
            {
                await this.listVolumeService.RemoveVolumeFromList(this.User.GetId(), id);

                return this.NoContent();
            }
            catch (KeyNotFoundException ex)
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
