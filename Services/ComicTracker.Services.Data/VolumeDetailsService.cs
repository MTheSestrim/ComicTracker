namespace ComicTracker.Services.Data
{
    using System.Linq;
    using System.Security.Claims;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Volume;

    using Microsoft.AspNetCore.Http;

    public class VolumeDetailsService : IVolumeDetailsService
    {
        private readonly IDeletableEntityRepository<Volume> volumesRepository;
        private readonly IDeletableEntityRepository<Issue> issuesRepository;
        private readonly IDeletableEntityRepository<Arc> arcsRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public VolumeDetailsService(
            IDeletableEntityRepository<Volume> volumesRepository,
            IDeletableEntityRepository<Issue> issuesRepository,
            IDeletableEntityRepository<Arc> arcsRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            this.volumesRepository = volumesRepository;
            this.issuesRepository = issuesRepository;
            this.arcsRepository = arcsRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public VolumeDetailsServiceModel GetVolume(int volumeId)
        {
            // Entities are extracted in separate queries to take advantage of IQueryable.
            // Otherwise, selecting and ordering is done in-memory, returning IEnumerable and slowing down app.
            var issues = this.issuesRepository
                .All()
                .Where(i => i.VolumeId == volumeId)
                .Select(i => new EntityLinkingModel
                {
                    Id = i.Id,
                    CoverPath = i.CoverPath,
                    Title = i.Title,
                    Number = i.Number,
                }).OrderByDescending(i => i.Number).ToArray();

            var arcs = this.arcsRepository
                .All()
                .Where(a => a.ArcsVolumes.Any(av => av.VolumeId == volumeId))
                .Select(a => new EntityLinkingModel
                {
                    Id = a.Id,
                    CoverPath = a.CoverPath,
                    Title = a.Title,
                    Number = a.Number,
                }).OrderByDescending(a => a.Number).ToArray();

            var userId = this.httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var currentVolume = this.volumesRepository.All()
                .Select(v => new VolumeDetailsServiceModel
                {
                    Id = v.Id,
                    Title = v.Title,
                    CoverPath = v.CoverPath,
                    Description = v.Description,
                    Number = v.Number,
                    TotalScore = v.UsersVolumes.Average(uv => uv.Score).ToString(),
                    UserScore = v.UsersVolumes.FirstOrDefault(uv => uv.UserId == userId).Score.ToString(),
                    SeriesId = v.SeriesId,
                    SeriesTitle = v.Series.Name,
                    Issues = issues,
                    Arcs = arcs,
                })
                .FirstOrDefault(v => v.Id == volumeId);

            if (currentVolume == null)
            {
                return null;
            }

            return currentVolume;
        }
    }
}
