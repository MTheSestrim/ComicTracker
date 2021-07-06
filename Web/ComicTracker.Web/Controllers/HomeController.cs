﻿namespace ComicTracker.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IHomePageService homePageService;

        public HomeController(IHomePageService homePageService)
        {
            this.homePageService = homePageService;
        }

        public async Task<IActionResult> Index()
        {
            var series = await this.homePageService.GetSeriesAsync();

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
