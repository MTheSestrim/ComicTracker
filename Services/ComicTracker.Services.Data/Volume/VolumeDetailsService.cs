namespace ComicTracker.Services.Data.Volume
{
    using System.Linq;
    using System.Security.Claims;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Volume.Contracts;
    using ComicTracker.Services.Data.Volume.Models;
    using Microsoft.AspNetCore.Http;

    public class VolumeDetailsService : IVolumeDetailsService
    {
        private readonly IDeletableEntityRepository<Volume> volumesRepository;
        private readonly IDeletableEntityRepository<Issue> issuesRepository;
        private readonly IDeletableEntityRepository<Arc> arcsRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;

        public VolumeDetailsService(
            IDeletableEntityRepository<Volume> volumesRepository,
            IDeletableEntityRepository<Issue> issuesRepository,
            IDeletableEntityRepository<Arc> arcsRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            this.volumesRepository = volumesRepository;
            this.issuesRepository = issuesRepository;
            this.arcsRepository = arcsRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
        }

        public VolumeDetailsServiceModel GetVolume(int volumeId)
        {
            // Entities are extracted in separate queries to take advantage of IQueryable.
            // Otherwise, selecting and ordering is done in-memory, returning IEnumerable and slowing down app.
            var issues = this.issuesRepository
                .AllAsNoTracking()
                .Where(i => i.VolumeId == volumeId)
                .ProjectTo<EntityLinkingModel>(this.mapper.ConfigurationProvider)
                .OrderByDescending(i => i.Number).ToArray();

            var arcs = this.arcsRepository
                .AllAsNoTracking()
                .Where(a => a.ArcsVolumes.Any(av => av.VolumeId == volumeId))
                .ProjectTo<EntityLinkingModel>(this.mapper.ConfigurationProvider)
                .OrderByDescending(a => a.Number).ToArray();

            var userId = this.httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            /* Mapper is not used;
             * 1. A separate query is necessary for taking UserScore and setting it later;
             * 2. Another option is taking the user in the MappingProfile, but that creates tight coupling.
             */
            var currentVolume = this.volumesRepository
                .AllAsNoTracking()
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
                    SeriesTitle = v.Series.Title,
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
