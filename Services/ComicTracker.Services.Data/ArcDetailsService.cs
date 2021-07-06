namespace ComicTracker.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Web.ViewModels.Arc;
    using ComicTracker.Web.ViewModels.Entities;

    using Microsoft.EntityFrameworkCore;

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

        public async Task<ArcDetailsViewModel> GetArcAsync(int arcId)
        {
            // Entities are extracted in separate queries to take advantage of IQueryable.
            // Otherwise, selecting and ordering is done in-memory, returning IEnumerable and slowing down app.
            var issues = await this.issuesRepository
                .All()
                .Where(i => i.ArcId == arcId)
                .Select(i => new EntityLinkingModel
                {
                    Id = i.Id,
                    CoverPath = i.CoverPath,
                    Title = i.Title,
                    Number = i.Number,
                }).OrderByDescending(i => i.Number).ToArrayAsync();

            var volumes = await this.volumesRepository
                .All()
                .Where(v => v.ArcsVolumes.Any(av => av.ArcId == arcId))
                .Select(v => new EntityLinkingModel
                {
                    Id = v.Id,
                    CoverPath = v.CoverPath,
                    Title = v.Title,
                    Number = v.Number,
                }).OrderByDescending(v => v.Number).ToArrayAsync();

            var currentArc = await this.arcsRepository.All()
                .Select(a => new ArcDetailsViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    CoverPath = a.CoverPath,
                    Description = a.Description,
                    Number = a.Number,
                    SeriesId = a.SeriesId,
                    SeriesTitle = a.Series.Name,
                    Issues = issues,
                    Volumes = volumes,
                })
                .FirstOrDefaultAsync(a => a.Id == arcId);

            if (currentArc == null)
            {
                return null;
            }

            return currentArc;
        }
    }
}
