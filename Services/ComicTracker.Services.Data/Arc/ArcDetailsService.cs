namespace ComicTracker.Services.Data.Arc
{
    using System.Linq;
    using System.Security.Claims;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using ComicTracker.Data;
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Arc.Models;
    using ComicTracker.Services.Data.Models.Entities;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class ArcDetailsService : IArcDetailsService
    {
        private readonly ComicTrackerDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;

        public ArcDetailsService(ComicTrackerDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
        }

        public ArcDetailsServiceModel GetArc(int arcId)
        {
            // Entities are extracted in separate queries to take advantage of IQueryable.
            // Otherwise, selecting and ordering is done in-memory, returning IEnumerable and slowing down app.
            var issues = this.dbContext.Issues
                .AsNoTracking()
                .Where(i => i.ArcId == arcId)
                .ProjectTo<EntityLinkingModel>(this.mapper.ConfigurationProvider)
                .OrderByDescending(i => i.Number).ToArray();

            var volumes = this.dbContext.Volumes
                .AsNoTracking()
                .Where(v => v.ArcsVolumes.Any(av => av.ArcId == arcId))
                .ProjectTo<EntityLinkingModel>(this.mapper.ConfigurationProvider)
                .OrderByDescending(v => v.Number).ToArray();

            var userId = this.httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            /* Mapper is not used;
             * 1. A separate query is necessary for taking UserScore and setting it later;
             * 2. Another option is taking the user in the MappingProfile, but that creates tight coupling.
             */
            var currentArc = this.dbContext.Arcs
                .AsNoTracking()
                .Select(a => new ArcDetailsServiceModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    CoverPath = a.CoverPath,
                    Description = a.Description,
                    Number = a.Number,
                    TotalScore = a.UsersArcs.Average(ua => ua.Score).ToString(),
                    UserScore = a.UsersArcs.FirstOrDefault(ua => ua.UserId == userId).Score.ToString(),
                    SeriesId = a.SeriesId,
                    SeriesTitle = a.Series.Title,
                    Issues = issues,
                    Volumes = volumes,
                })
                .FirstOrDefault(a => a.Id == arcId);

            if (currentArc == null)
            {
                return null;
            }

            return currentArc;
        }
    }
}
