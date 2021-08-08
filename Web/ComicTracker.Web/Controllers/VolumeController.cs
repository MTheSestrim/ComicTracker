namespace ComicTracker.Web.Controllers
{
    using System;

    using ComicTracker.Services.Data.Volume.Contracts;
    using ComicTracker.Services.Data.Volume.Models;
    using ComicTracker.Web.Infrastructure;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using static ComicTracker.Common.CacheConstants;

    public class VolumeController : BaseController
    {
        private readonly IVolumeDetailsService volumeDetailsService;
        private readonly IMemoryCache cache;

        public VolumeController(IVolumeDetailsService volumeDetailsService, IMemoryCache cache)
        {
            this.volumeDetailsService = volumeDetailsService;
            this.cache = cache;
        }

        public IActionResult Index(int id)
        {
            var cacheKey = VolumeDetailsCacheKey + id.ToString();

            var currentVolume = this.cache.Get<VolumeDetailsServiceModel>(cacheKey);

            if (currentVolume == null)
            {
                currentVolume = this.volumeDetailsService.GetVolume(id, this.User.GetId());

                if (currentVolume == null)
                {
                    return this.NotFound(currentVolume);
                }

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                this.cache.Set(cacheKey, currentVolume, cacheOptions);
            }

            return this.View(currentVolume);
        }
    }
}
