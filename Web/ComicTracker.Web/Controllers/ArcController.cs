namespace ComicTracker.Core.Controllers
{
    using ComicTracker.Web.Controllers;
    using Microsoft.AspNetCore.Mvc;

    public class ArcController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
