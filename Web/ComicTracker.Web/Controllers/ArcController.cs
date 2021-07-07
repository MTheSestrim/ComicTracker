namespace ComicTracker.Web.Controllers
{
    using ComicTracker.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class ArcController : BaseController
    {
        private readonly IArcDetailsService arcDetailsService;

        public ArcController(IArcDetailsService arcDetailsService)
        {
            this.arcDetailsService = arcDetailsService;
        }

        public IActionResult Index(int id)
        {
            var currentArc = this.arcDetailsService.GetArc(id);

            if (currentArc == null)
            {
                return this.NotFound(currentArc);
            }

            return this.View(currentArc);
        }
    }
}
