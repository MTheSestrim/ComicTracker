namespace ComicTracker.Web.Controllers
{
    using System.Linq;

    using ComicTracker.Data;

    using ComicTracker.Web.ViewModels.Series;

    using Microsoft.AspNetCore.Mvc;

    public class SeriesController : BaseController
    {
        private readonly ComicTrackerDbContext context;

        public SeriesController(ComicTrackerDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index(int id)
        {
            var currentSeries = this.context.Series
               .Select(s => new SeriesModel
               {
                   Id = s.Id,
                   Title = s.Name,
                   CoverPath = s.CoverPath,
                   Ongoing = s.Ongoing,
                   Description = s.Description,
               })
               .FirstOrDefault(s => s.Id == id);

            if (currentSeries == null)
            {
                return this.NotFound(currentSeries);
            }

            // Entities are extracted in separate queries to take advantage of IQueryable.
            // Otherwise, selecting and ordering is done in-memory, returning IEnumerable and slowing down app.
            // As an added bonus, this approach takes advantage of caching.
            var issues = this.context.Issues.
               Where(i => i.Series.Id == currentSeries.Id)
               .Select(i => new EntityLinkingModel
               {
                   CoverPath = i.CoverPath,
                   Title = i.Title,
                   Number = i.Number,
               }).OrderByDescending(i => i.Number).ToArray();

            var arcs = this.context.Arcs.
               Where(a => a.Series.Id == currentSeries.Id)
               .Select(a => new EntityLinkingModel
               {
                   CoverPath = a.CoverPath,
                   Title = a.Title,
                   Number = a.Number,
               }).OrderByDescending(a => a.Number).ToArray();

            var volumes = this.context.Volumes.
               Where(v => v.Series.Id == currentSeries.Id)
               .Select(v => new EntityLinkingModel
               {
                   CoverPath = v.CoverPath,
                   Title = v.Title,
                   Number = v.Number,
               }).OrderByDescending(i => i.Number).ToArray();

            currentSeries.Issues = issues;
            currentSeries.Arcs = arcs;
            currentSeries.Volumes = volumes;

            return this.View(currentSeries);
        }
    }
}
