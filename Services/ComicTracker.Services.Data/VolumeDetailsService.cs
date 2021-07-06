﻿namespace ComicTracker.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Web.ViewModels.Entities;
    using ComicTracker.Web.ViewModels.Volume;

    using Microsoft.EntityFrameworkCore;

    public class VolumeDetailsService : IVolumeDetailsService
    {
        private readonly IDeletableEntityRepository<Volume> volumesRepository;
        private readonly IDeletableEntityRepository<Issue> issuesRepository;
        private readonly IDeletableEntityRepository<Arc> arcsRepository;

        public VolumeDetailsService(
            IDeletableEntityRepository<Volume> volumesRepository,
            IDeletableEntityRepository<Issue> issuesRepository,
            IDeletableEntityRepository<Arc> arcsRepository)
        {
            this.volumesRepository = volumesRepository;
            this.issuesRepository = issuesRepository;
            this.arcsRepository = arcsRepository;
        }

        public async Task<VolumeDetailsViewModel> GetVolumeAsync(int volumeId)
        {
            // Entities are extracted in separate queries to take advantage of IQueryable.
            // Otherwise, selecting and ordering is done in-memory, returning IEnumerable and slowing down app.
            var issues = await this.issuesRepository
                .All()
                .Where(i => i.VolumeId == volumeId)
                .Select(i => new EntityLinkingModel
                {
                    Id = i.Id,
                    CoverPath = i.CoverPath,
                    Title = i.Title,
                    Number = i.Number,
                }).OrderByDescending(i => i.Number).ToArrayAsync();

            var arcs = await this.arcsRepository
                .All()
                .Where(a => a.ArcsVolumes.Any(av => av.VolumeId == volumeId))
                .Select(a => new EntityLinkingModel
                {
                    Id = a.Id,
                    CoverPath = a.CoverPath,
                    Title = a.Title,
                    Number = a.Number,
                }).OrderByDescending(a => a.Number).ToArrayAsync();

            var currentVolume = await this.volumesRepository.All()
                .Select(v => new VolumeDetailsViewModel
                {
                    Id = v.Id,
                    Title = v.Title,
                    CoverPath = v.CoverPath,
                    Description = v.Description,
                    Number = v.Number,
                    SeriesId = v.SeriesId,
                    SeriesTitle = v.Series.Name,
                    Issues = issues,
                    Arcs = arcs,
                })
                .FirstOrDefaultAsync(v => v.Id == volumeId);

            if (currentVolume == null)
            {
                return null;
            }

            return currentVolume;
        }
    }
}
