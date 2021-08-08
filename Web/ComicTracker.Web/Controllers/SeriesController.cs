namespace ComicTracker.Web.Controllers
{
    using System;

    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Services.Data.Series.Models;
    using ComicTracker.Web.Infrastructure;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using static ComicTracker.Common.CacheConstants;

    public class SeriesController : BaseController
    {
        private readonly ISeriesDetailsService seriesDetailsService;
        private readonly IMemoryCache cache;

        public SeriesController(ISeriesDetailsService seriesDetailsService, IMemoryCache cache)
        {
            this.seriesDetailsService = seriesDetailsService;
            this.cache = cache;
        }

        public IActionResult Index(int id)
        {
            var cacheKey = SeriesDetailsCacheKey + id.ToString();

            var currentSeries = this.cache.Get<SeriesDetailsServiceModel>(cacheKey);

            if (currentSeries == null)
            {
                currentSeries = this.seriesDetailsService.GetSeries(id, this.User.GetId());

                if (currentSeries == null)
                {
                    return this.NotFound(currentSeries);
                }

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(3));

                this.cache.Set(cacheKey, currentSeries, cacheOptions);
            }

            return this.View(currentSeries);
        }
    }
}
