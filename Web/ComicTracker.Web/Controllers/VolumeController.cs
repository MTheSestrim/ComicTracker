namespace ComicTracker.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class VolumeController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
