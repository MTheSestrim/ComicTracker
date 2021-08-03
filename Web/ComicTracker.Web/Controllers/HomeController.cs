namespace ComicTracker.Web.Controllers
{
    using System.Diagnostics;

    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Web.ViewModels;
    using ComicTracker.Web.ViewModels.Home;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly ISeriesSearchQueryingService homePageService;

        public HomeController(ISeriesSearchQueryingService homePageService)
        {
            this.homePageService = homePageService;
        }

        public IActionResult Index([FromQuery]HomePageViewModel model)
        {
            model.TotalSeriesCount = this.homePageService.GetTotalSeriesCount(model.SearchTerm, model.Sorting);

            if (model.CurrentPage > model.MaxPageCount)
            {
                model.CurrentPage = (int)model.MaxPageCount;
                return this.RedirectToAction("Index", new { model.Sorting, model.CurrentPage, model.SearchTerm });
            }

            model.Series = this.homePageService.GetSeries(model.CurrentPage, model.SearchTerm, model.Sorting);

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
