namespace ComicTracker.Services.Data.Volume
{
    using System.Linq;
    using System.Security.Claims;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using ComicTracker.Data;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Volume.Contracts;
    using ComicTracker.Services.Data.Volume.Models;

    using Microsoft.EntityFrameworkCore;

    public class VolumeDetailsService : IVolumeDetailsService
    {
        private readonly ComicTrackerDbContext dbContext;
        private readonly IMapper mapper;

        public VolumeDetailsService(ComicTrackerDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public VolumeDetailsServiceModel GetVolume(int volumeId, string userId)
        {
            // Entities are extracted in separate queries to take advantage of IQueryable.
            // Otherwise, selecting and ordering is done in-memory, returning IEnumerable and slowing down app.
            var issues = this.dbContext.Issues
                .AsNoTracking()
                .Where(i => i.VolumeId == volumeId)
                .ProjectTo<EntityLinkingModel>(this.mapper.ConfigurationProvider)
                .OrderByDescending(i => i.Number).ToArray();

            var arcs = this.dbContext.Arcs
                .AsNoTracking()
                .Where(a => a.ArcsVolumes.Any(av => av.VolumeId == volumeId))
                .ProjectTo<EntityLinkingModel>(this.mapper.ConfigurationProvider)
                .OrderByDescending(a => a.Number).ToArray();

            var currentVolume = this.dbContext.Volumes
                .AsNoTracking()
                .Select(v => new VolumeDetailsServiceModel
                {
                    Id = v.Id,
                    Title = v.Title,
                    CoverPath = v.CoverPath,
                    Description = v.Description,
                    Number = v.Number,
                    IsInList = v.UsersVolumes.Any(uv => uv.UserId == userId),
                    TotalScore = v.UsersVolumes.Average(uv => uv.Score).ToString(),
                    UserScore = v.UsersVolumes.FirstOrDefault(uv => uv.UserId == userId).Score.ToString(),
                    SeriesId = v.SeriesId,
                    SeriesTitle = v.Series.Title,
                    Issues = issues,
                    Arcs = arcs,
                    Genres = v.Genres.Select(g => new NameOnlyLinkingModel { Id = g.Id, Name = g.Name }).ToList(),
                    Artists = v.Artists.Select(a => new NameOnlyLinkingModel { Id = a.Id, Name = a.Name }).ToList(),
                    Writers = v.Writers.Select(w => new NameOnlyLinkingModel { Id = w.Id, Name = w.Name }).ToList(),
                    Characters = v.CharactersVolumes
                    .Select(cv => new NameOnlyLinkingModel
                    {
                        Id = cv.Character.Id,
                        Name = cv.Character.Name,
                    }).ToList(),
                    Publishers = v.PublishersVolumes
                    .Select(pv => new PublisherLinkingModel
                    {
                        Id = pv.Publisher.Id,
                        Name = pv.Publisher.Name,
                        Nationality = pv.Publisher.Nationality.Name,
                    }).ToList(),
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
