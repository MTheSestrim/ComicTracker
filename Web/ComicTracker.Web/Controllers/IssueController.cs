namespace ComicTracker.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class IssueController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
