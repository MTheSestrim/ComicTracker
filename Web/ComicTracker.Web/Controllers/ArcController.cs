namespace ComicTracker.Web.Controllers
{
    using ComicTracker.Services.Contracts;
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.Infrastructure.IMemoryCacheExtensions;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class ArcController : BaseController
    {
        private readonly IArcDetailsService arcDetailsService;
        private readonly IMemoryCache cache;
        private readonly ICacheKeyHolderService<int> cacheKeyHolder;

        public ArcController(
            IArcDetailsService arcDetailsService,
            IMemoryCache cache,
            ICacheKeyHolderService<int> cacheKeyHolder)
        {
            this.arcDetailsService = arcDetailsService;
            this.cache = cache;
            this.cacheKeyHolder = cacheKeyHolder;
        }

        public IActionResult Index(int id)
        {
            var currentArc = this.cache.GetArcDetails(id);

            if (currentArc == null)
            {
                currentArc = this.arcDetailsService.GetArc(id, this.User.GetId());

                if (currentArc == null)
                {
                    return this.NotFound();
                }

                this.cache.SetArcDetails(currentArc, this.cacheKeyHolder);
            }

            return this.View(currentArc);
        }
    }
}
