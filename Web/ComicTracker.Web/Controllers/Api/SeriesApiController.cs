namespace ComicTracker.Web.Controllers.Api
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.List.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.Infrastructure.IMemoryCacheExtensions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    [Authorize]
    [ApiController]
    [Route("api/Series")]
    public class SeriesApiController : BaseController
    {
        private readonly ISeriesRatingService seriesRatingService;
        private readonly IListSeriesService listSeriesService;
        private readonly IMemoryCache cache;

        public SeriesApiController(
            ISeriesRatingService seriesRatingService,
            IListSeriesService listSeriesService,
            IMemoryCache cache)
        {
            this.seriesRatingService = seriesRatingService;
            this.listSeriesService = listSeriesService;
            this.cache = cache;
        }

        [HttpPut]
        [Route("Score")]
        public async Task<ActionResult<int?>> ScoreSeries(RateApiRequestModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            this.cache.RemoveSeriesDetails(model.Id);

            return await this.seriesRatingService.RateSeries(this.User.GetId(), model);
        }

        [HttpPut]
        [Route("AddToList/{id}")]
        public async Task<ActionResult> AddSeriesToList(int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            this.cache.RemoveSeriesDetails(id);

            try
            {
                await this.listSeriesService.AddSeriesToList(this.User.GetId(), id);

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
        public async Task<ActionResult> RemoveSeriesFromList(int id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            this.cache.RemoveSeriesDetails(id);

            try
            {
                await this.listSeriesService.RemoveSeriesFromList(this.User.GetId(), id);

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
