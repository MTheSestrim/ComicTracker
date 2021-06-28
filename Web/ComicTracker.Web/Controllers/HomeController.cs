namespace ComicTracker.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;

    using ComicTracker.Data;
    using ComicTracker.Web.ViewModels;
    using ComicTracker.Web.ViewModels.Home;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly ComicTrackerDbContext context;

        public HomeController(ComicTrackerDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var series = this.context
                .Series
                .Select(s => new HomeSeriesViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    CoverPath = s.CoverPath,
                    IssuesCount = s.Issues.Count,
                })
                .ToList();

            return this.View(series);
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
