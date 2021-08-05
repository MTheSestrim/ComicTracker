namespace ComicTracker.Services.Data.Series
{
    using System.Linq;
    using System.Security.Claims;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using ComicTracker.Data;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Services.Data.Series.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    public class SeriesDetailsService : ISeriesDetailsService
    {
        private readonly ComicTrackerDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;

        public SeriesDetailsService(ComicTrackerDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
        }

        public SeriesDetailsServiceModel GetSeries(
            int seriesId)
        {
            // Entities are extracted in separate queries to take advantage of IQueryable.
            // Otherwise, selecting and ordering is done in-memory, returning IEnumerable and slowing down app.
            var issues = this.dbContext.Issues
                .AsNoTracking()
                .Where(i => i.SeriesId == seriesId)
                .ProjectTo<EntityLinkingModel>(this.mapper.ConfigurationProvider)
                .OrderByDescending(i => i.Number).ToArray();

            var volumes = this.dbContext.Volumes
                .AsNoTracking()
                .Where(v => v.SeriesId == seriesId)
                .ProjectTo<EntityLinkingModel>(this.mapper.ConfigurationProvider)
                .OrderByDescending(i => i.Number).ToArray();

            var arcs = this.dbContext.Arcs
                .AsNoTracking()
                .Where(a => a.SeriesId == seriesId)
                .ProjectTo<EntityLinkingModel>(this.mapper.ConfigurationProvider)
                .OrderByDescending(a => a.Number).ToArray();

            var userId = this.httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var currentSeries = this.dbContext.Series
               .AsNoTracking()
               .Select(s => new SeriesDetailsServiceModel
               {
                   Id = s.Id,
                   Title = s.Title,
                   CoverPath = s.CoverPath,
                   Ongoing = s.Ongoing,
                   Description = s.Description,
                   TotalScore = s.UsersSeries.Average(us => us.Score).ToString(),
                   UserScore = s.UsersSeries.FirstOrDefault(us => us.UserId == userId).Score.ToString(),
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
