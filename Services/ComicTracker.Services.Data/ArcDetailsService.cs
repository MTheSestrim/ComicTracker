﻿namespace ComicTracker.Services.Data
{
    using System.Linq;
    using System.Security.Claims;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Services.Data.Models.Arc;
    using ComicTracker.Services.Data.Models.Entities;
    using Microsoft.AspNetCore.Http;

    public class ArcDetailsService : IArcDetailsService
    {
        private readonly IDeletableEntityRepository<Arc> arcsRepository;
        private readonly IDeletableEntityRepository<Issue> issuesRepository;
        private readonly IDeletableEntityRepository<Volume> volumesRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ArcDetailsService(
            IDeletableEntityRepository<Arc> arcsRepository,
            IDeletableEntityRepository<Issue> issuesRepository,
            IDeletableEntityRepository<Volume> volumesRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            this.arcsRepository = arcsRepository;
            this.issuesRepository = issuesRepository;
            this.volumesRepository = volumesRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public ArcDetailsServiceModel GetArc(int arcId)
        {
            // Entities are extracted in separate queries to take advantage of IQueryable.
            // Otherwise, selecting and ordering is done in-memory, returning IEnumerable and slowing down app.
            var issues = this.issuesRepository
                .All()
                .Where(i => i.ArcId == arcId)
                .Select(i => new EntityLinkingModel
                {
                    Id = i.Id,
                    CoverPath = i.CoverPath,
                    Title = i.Title,
                    Number = i.Number,
                }).OrderByDescending(i => i.Number).ToArray();

            var volumes = this.volumesRepository
                .All()
                .Where(v => v.ArcsVolumes.Any(av => av.ArcId == arcId))
                .Select(v => new EntityLinkingModel
                {
                    Id = v.Id,
                    CoverPath = v.CoverPath,
                    Title = v.Title,
                    Number = v.Number,
                }).OrderByDescending(v => v.Number).ToArray();

            var userId = this.httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var currentArc = this.arcsRepository.All()
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
                    SeriesTitle = a.Series.Name,
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
