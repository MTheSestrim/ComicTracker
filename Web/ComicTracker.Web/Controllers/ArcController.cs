namespace ComicTracker.Web.Controllers
{
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Web.Infrastructure;

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
            var currentArc = this.arcDetailsService.GetArc(id, this.User.GetId());

            if (currentArc == null)
            {
                return this.NotFound(currentArc);
            }

            return this.View(currentArc);
        }
    }
}
