namespace ComicTracker.Web.Controllers
{
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Web.ViewModels.Error;
    using ComicTracker.Web.ViewModels.Home;

    using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Error(int? statusCode = null)
        {
            return this.View(
                new StatusCodeErrorViewModel { StatusCode = statusCode.Value });
        }
    }
}
