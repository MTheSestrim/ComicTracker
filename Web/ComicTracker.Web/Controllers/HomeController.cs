namespace ComicTracker.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using ComicTracker.Services.Data.Models.Home;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Web.ViewModels;
    using ComicTracker.Web.ViewModels.Home;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using static ComicTracker.Common.CacheConstants;

    public class HomeController : BaseController
    {
        private readonly ISeriesSearchQueryingService homePageService;
        private readonly IMemoryCache cache;

        public HomeController(ISeriesSearchQueryingService homePageService, IMemoryCache cache)
        {
            this.homePageService = homePageService;
            this.cache = cache;
        }

        public IActionResult Index([FromQuery] HomePageViewModel model)
        {
            var totalSeriesCount = this.cache.Get<int>(HomeCountCacheKey);

            if (totalSeriesCount == 0)
            {
                totalSeriesCount = this.homePageService.GetTotalSeriesCount(model.SearchTerm, model.Sorting);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                this.cache.Set(HomeCountCacheKey, totalSeriesCount, cacheOptions);
            }

            model.TotalSeriesCount = totalSeriesCount;

            if (model.CurrentPage < 1)
            {
                model.CurrentPage = 1;
                return this.RedirectToAction("Index", new { model.Sorting, model.CurrentPage, model.SearchTerm });
            }

            if (model.CurrentPage > model.MaxPageCount)
            {
                model.CurrentPage = (int)model.MaxPageCount;
                return this.RedirectToAction("Index", new { model.Sorting, model.CurrentPage, model.SearchTerm });
            }

            var series = this.homePageService.GetSeries(model.CurrentPage, model.SearchTerm, model.Sorting);

            model.Series = series;

            return this.View(model);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
