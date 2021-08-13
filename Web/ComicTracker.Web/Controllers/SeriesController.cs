namespace ComicTracker.Web.Controllers
{
    using ComicTracker.Services.Contracts;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Web.Infrastructure;
    using ComicTracker.Web.Infrastructure.IMemoryCacheExtensions;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class SeriesController : BaseController
    {
        private readonly ISeriesDetailsService seriesDetailsService;
        private readonly IMemoryCache cache;
        private readonly ICacheKeyHolderService<int> cacheKeyHolder;

        public SeriesController(
            ISeriesDetailsService seriesDetailsService,
            IMemoryCache cache,
            ICacheKeyHolderService<int> cacheKeyHolder)
        {
            this.seriesDetailsService = seriesDetailsService;
            this.cache = cache;
            this.cacheKeyHolder = cacheKeyHolder;
        }

        public IActionResult Index(int id)
        {
            var currentSeries = this.cache.GetSeriesDetails(id);

            if (currentSeries == null)
            {
                currentSeries = this.seriesDetailsService.GetSeries(id, this.User.GetId());

                if (currentSeries == null)
                {
                    return this.NotFound(currentSeries);
                }

                this.cache.SetSeriesDetails(currentSeries, this.cacheKeyHolder);
            }

            return this.View(currentSeries);
        }
    }
}
