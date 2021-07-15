namespace ComicTracker.Web.Controllers
{
    using System.Diagnostics;

    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Web.ViewModels;
    using ComicTracker.Web.ViewModels.Home;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly ISeriesRetrievalService homePageService;

        public HomeController(ISeriesRetrievalService homePageService)
        {
            this.homePageService = homePageService;
        }

        public IActionResult Index([FromQuery]HomePageViewModel model)
        {
            model.Series = this.homePageService.GetSeries(model.CurrentPage, model.SearchTerm);
            model.TotalSeriesCount = this.homePageService.GetTotalSeriesCount();

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
