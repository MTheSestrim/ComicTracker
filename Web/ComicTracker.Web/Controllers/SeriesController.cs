namespace ComicTracker.Core.Controllers
{
    using ComicTracker.Web.Controllers;
    using Microsoft.AspNetCore.Mvc;

    public class SeriesController : BaseController
    {
        public IActionResult Index()
        {
            ////TODO: Add DI
            // var context = new ComicTrackerContext();
            //
            // var currentSeries = context.Series
            //    .Select(s => new SeriesModel
            //    {
            //        Id = s.Id,
            //        Title = s.Name,
            //        CoverPath = s.CoverPath,
            //        Ongoing = s.Ongoing,
            //        Description = s.Description,
            //    })
            //    .First();
            //
            ////Entities are extracted in separate queries to take advantage of IQueryable.
            ////Otherwise, selecting and ordering is done in-memory, returning IEnumerable and slowing down app.
            ////As an added bonus, this approach takes advantage of caching.
            //
            // var issues = context.Issues.
            //    Where(i => i.Series.Id == currentSeries.Id)
            //    .Select(i => new EntityLinkingModel
            //    {
            //        CoverPath = i.CoverPath,
            //        Title = i.Title,
            //        Number = i.Number
            //    }).OrderByDescending(i => i.Number).ToArray();
            //
            // var arcs = context.Arcs.
            //    Where(a => a.Series.Id == currentSeries.Id)
            //    .Select(a => new EntityLinkingModel
            //    {
            //        CoverPath = a.CoverPath,
            //        Title = a.Title,
            //        Number = a.Number
            //    }).OrderByDescending(a => a.Number).ToArray();
            //
            // var volumes = context.Volumes.
            //    Where(v => v.Series.Id == currentSeries.Id)
            //    .Select(v => new EntityLinkingModel
            //    {
            //        CoverPath = v.CoverPath,
            //        Title = v.Title,
            //        Number = v.Number
            //    }).OrderByDescending(i => i.Number).ToArray();
            //
            // currentSeries.Issues = issues;
            // currentSeries.Arcs = arcs;
            // currentSeries.Volumes = volumes;
            //
            // return this.View(currentSeries);
            return this.View();
        }
    }
}
