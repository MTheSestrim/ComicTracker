namespace ComicTracker.Services.Data
{
    using System.Linq;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Web.ViewModels.Arc;
    using ComicTracker.Web.ViewModels.Entities;

    public class ArcDetailsService : IArcDetailsService
    {
        private readonly IDeletableEntityRepository<Arc> arcsRepository;
        private readonly IDeletableEntityRepository<Issue> issuesRepository;
        private readonly IDeletableEntityRepository<Volume> volumesRepository;

        public ArcDetailsService(
            IDeletableEntityRepository<Arc> arcsRepository,
            IDeletableEntityRepository<Issue> issuesRepository,
            IDeletableEntityRepository<Volume> volumesRepository)
        {
            this.arcsRepository = arcsRepository;
            this.issuesRepository = issuesRepository;
            this.volumesRepository = volumesRepository;
        }

        public ArcDetailsViewModel GetArc(int arcId)
        {
            var currentArc = this.arcsRepository.All()
                .Select(a => new ArcDetailsViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    CoverPath = a.CoverPath,
                    Description = a.Description,
                    Number = a.Number,
                    SeriesId = a.SeriesId,
                    SeriesTitle = a.Series.Name,
                })
                .FirstOrDefault(a => a.Id == arcId);

            if (currentArc == null)
            {
                return null;
            }

            // Entities are extracted in separate queries to take advantage of IQueryable.
            // Otherwise, selecting and ordering is done in-memory, returning IEnumerable and slowing down app.
            var issues = this.issuesRepository
                .All()
                .Where(i => i.ArcId == currentArc.Id)
                .Select(i => new EntityLinkingModel
                {
                    Id = i.Id,
                    CoverPath = i.CoverPath,
                    Title = i.Title,
                    Number = i.Number,
                }).OrderByDescending(i => i.Number).ToArray();

            var volumes = this.volumesRepository
                .All()
                .Where(v => v.ArcsVolumes.Any(av => av.ArcId == currentArc.Id))
                .Select(v => new EntityLinkingModel
                {
                    Id = v.Id,
                    CoverPath = v.CoverPath,
                    Title = v.Title,
                    Number = v.Number,
                }).OrderByDescending(v => v.Number).ToArray();

            currentArc.Issues = issues;
            currentArc.Volumes = volumes;

            return currentArc;
        }
    }
}
