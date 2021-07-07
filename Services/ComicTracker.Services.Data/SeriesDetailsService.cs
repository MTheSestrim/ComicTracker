namespace ComicTracker.Services.Data
{
    using System.Linq;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Web.ViewModels.Entities;
    using ComicTracker.Web.ViewModels.Series;

    public class SeriesDetailsService : ISeriesDetailsService
    {
        private readonly IDeletableEntityRepository<Series> seriesRepository;
        private readonly IDeletableEntityRepository<Issue> issuesRepository;
        private readonly IDeletableEntityRepository<Arc> arcsRepository;
        private readonly IDeletableEntityRepository<Volume> volumesRepository;

        public SeriesDetailsService(
            IDeletableEntityRepository<Series> seriesRepository,
            IDeletableEntityRepository<Issue> issuesRepository,
            IDeletableEntityRepository<Arc> arcsRepository,
            IDeletableEntityRepository<Volume> volumesRepository)
        {
            this.seriesRepository = seriesRepository;
            this.issuesRepository = issuesRepository;
            this.arcsRepository = arcsRepository;
            this.volumesRepository = volumesRepository;
        }

        public SeriesDetailsViewModel GetSeries(
            int seriesId)
        {
            // Entities are extracted in separate queries to take advantage of IQueryable.
            // Otherwise, selecting and ordering is done in-memory, returning IEnumerable and slowing down app.
            var issues = this.issuesRepository
                .All()
                .Where(i => i.SeriesId == seriesId)
                .Select(i => new EntityLinkingModel
                {
                    Id = i.Id,
                    CoverPath = i.CoverPath,
                    Title = i.Title,
                    Number = i.Number,
                }).OrderByDescending(i => i.Number).ToArray();

            var volumes = this.volumesRepository
                .All()
                .Where(v => v.SeriesId == seriesId)
                .Select(v => new EntityLinkingModel
                {
                    Id = v.Id,
                    CoverPath = v.CoverPath,
                    Title = v.Title,
                    Number = v.Number,
                }).OrderByDescending(i => i.Number).ToArray();

            var arcs = this.arcsRepository
                .All()
                .Where(a => a.SeriesId == seriesId)
                .Select(a => new EntityLinkingModel
                {
                    Id = a.Id,
                    CoverPath = a.CoverPath,
                    Title = a.Title,
                    Number = a.Number,
                }).OrderByDescending(a => a.Number).ToArray();

            var currentSeries = this.seriesRepository
               .All()
               .Select(s => new SeriesDetailsViewModel
               {
                   Id = s.Id,
                   Title = s.Name,
                   CoverPath = s.CoverPath,
                   Ongoing = s.Ongoing,
                   Description = s.Description,
                   Issues = issues,
                   Volumes = volumes,
                   Arcs = arcs,
               })
               .FirstOrDefault(s => s.Id == seriesId);

            if (currentSeries == null)
            {
                return null;
            }

            return currentSeries;
        }
    }
}
