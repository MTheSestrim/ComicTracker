namespace ComicTracker.Web.Controllers
{
    using System;

    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Arc.Models;
    using ComicTracker.Web.Infrastructure;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using static ComicTracker.Common.CacheConstants;

    public class ArcController : BaseController
    {
        private readonly IArcDetailsService arcDetailsService;
        private readonly IMemoryCache cache;

        public ArcController(IArcDetailsService arcDetailsService, IMemoryCache cache)
        {
            this.arcDetailsService = arcDetailsService;
            this.cache = cache;
        }

        public IActionResult Index(int id)
        {
            var cacheKey = ArcDetailsCacheKey + id.ToString();

            var currentArc = this.cache.Get<ArcDetailsServiceModel>(cacheKey);

            if (currentArc == null)
            {
                currentArc = this.arcDetailsService.GetArc(id, this.User.GetId());

                if (currentArc == null)
                {
                    return this.NotFound(currentArc);
                }

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                this.cache.Set(cacheKey, currentArc, cacheOptions);
            }

            return this.View(currentArc);
        }
    }
}
