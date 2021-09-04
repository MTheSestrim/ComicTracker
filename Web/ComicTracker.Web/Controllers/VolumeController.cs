namespace ComicTracker.Web.Controllers
{
    using ComicTracker.Services.Contracts;
    using ComicTracker.Services.Data.Volume.Contracts;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.Infrastructure.IMemoryCacheExtensions;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class VolumeController : BaseController
    {
        private readonly IVolumeDetailsService volumeDetailsService;
        private readonly IMemoryCache cache;
        private readonly ICacheKeyHolderService<int> cacheKeyHolder;

        public VolumeController(
            IVolumeDetailsService volumeDetailsService,
            IMemoryCache cache,
            ICacheKeyHolderService<int> cacheKeyHolder)
        {
            this.volumeDetailsService = volumeDetailsService;
            this.cache = cache;
            this.cacheKeyHolder = cacheKeyHolder;
        }

        public IActionResult Index(int id)
        {
            var currentVolume = this.cache.GetVolumeDetails(id);

            if (currentVolume == null)
            {
                currentVolume = this.volumeDetailsService.GetVolume(id, this.User.GetId());

                if (currentVolume == null)
                {
                    return this.NotFound();
                }

                this.cache.SetVolumeDetails(currentVolume, this.cacheKeyHolder);
            }

            return this.View(currentVolume);
        }
    }
}
