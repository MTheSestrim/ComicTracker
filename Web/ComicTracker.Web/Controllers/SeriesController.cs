namespace ComicTracker.Web.Controllers
{
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Web.Infrastructure;

    using Microsoft.AspNetCore.Mvc;

    public class SeriesController : BaseController
    {
        private readonly ISeriesDetailsService seriesDetailsService;

        public SeriesController(ISeriesDetailsService seriesDetailsService)
        {
            this.seriesDetailsService = seriesDetailsService;
        }

        public IActionResult Index(int id)
        {
            var currentSeries = this.seriesDetailsService.GetSeries(id, this.User.GetId());

            if (currentSeries == null)
            {
                return this.NotFound(currentSeries);
            }

            return this.View(currentSeries);
        }
    }
}
