namespace ComicTracker.Web.Controllers.Api
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.List.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.Infrastructure.IMemoryCacheExtensions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    [Authorize]
    [ApiController]
    [Route("api/Arc")]
    public class ArcApiController : BaseController
    {
        private readonly IArcRatingService arcRatingService;
        private readonly IListArcService listArcService;
        private readonly IMemoryCache cache;

        public ArcApiController(
            IArcRatingService arcRatingService,
            IListArcService listArcService,
            IMemoryCache cache)
        {
            this.arcRatingService = arcRatingService;
            this.listArcService = listArcService;
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

        [HttpPut]
        [Route("AddToList/{id}")]
        public async Task<ActionResult> AddArcToList(int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            this.cache.RemoveArcDetails(id);

            try
            {
                await this.listArcService.AddArcToList(this.User.GetId(), id);

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
        public async Task<ActionResult> RemoveArcFromList(int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            this.cache.RemoveArcDetails(id);

            try
            {
                await this.listArcService.RemoveArcFromList(this.User.GetId(), id);

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
