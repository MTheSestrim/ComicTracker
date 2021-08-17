namespace ComicTracker.Web.Controllers
{
    using System.Diagnostics;

    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Web.ViewModels;
    using ComicTracker.Web.ViewModels.Home;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    public class HomeController : BaseController
    {
        private readonly ISeriesSearchQueryingService homePageService;

        public HomeController(ISeriesSearchQueryingService homePageService)
        {
            this.homePageService = homePageService;
        }

        public IActionResult Index([FromQuery] HomePageViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var totalSeriesCount = this.homePageService.GetTotalSeriesCount(model.SearchTerm, model.Sorting);

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
