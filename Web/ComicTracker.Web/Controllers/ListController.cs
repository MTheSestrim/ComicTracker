namespace ComicTracker.Core.Controllers
{
    using ComicTracker.Web.Controllers;
    using Microsoft.AspNetCore.Mvc;

    public class ListController : BaseController
    {
        public ListController()
        {
        }

        public IActionResult Index()
        {
            ////TODO: Apply DI
            //
            // var listData = context
            //    .Series
            //    .Select(s => new ListModel
            //    {
            //        Title = s.Name,
            //        CoverPath = "https://cdn.myanimelist.net/images/manga/1/157897l.webp",
            //        Score = 10,
            //        IssueCount = s.Issues.Count,
            //        VolumeCount = s.Volumes.Count,
            //        ArcCount = s.Arcs.Count
            //    })
            //    .ToList();
            //
            // return this.View(listData);
            return this.View();
        }
    }
}
