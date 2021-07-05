﻿namespace ComicTracker.Services.Data
{
    using System.Linq;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Web.ViewModels.Entities;
    using ComicTracker.Web.ViewModels.Volume;

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

        public VolumeDetailsViewModel GetVolume(int volumeId)
        {
            var currentVolume = this.volumesRepository.All()
                .Select(v => new VolumeDetailsViewModel
                {
                    Id = v.Id,
                    Title = v.Title,
                    CoverPath = v.CoverPath,
                    Description = v.Description,
                    Number = v.Number,
                    SeriesId = v.SeriesId,
                    SeriesTitle = v.Series.Name,
                })
                .FirstOrDefault(v => v.Id == volumeId);

            if (currentVolume == null)
            {
                return null;
            }

            // Entities are extracted in separate queries to take advantage of IQueryable.
            // Otherwise, selecting and ordering is done in-memory, returning IEnumerable and slowing down app.
            var issues = this.issuesRepository
                .All()
                .Where(i => i.VolumeId == currentVolume.Id)
                .Select(i => new EntityLinkingModel
                {
                    Id = i.Id,
                    CoverPath = i.CoverPath,
                    Title = i.Title,
                    Number = i.Number,
                }).OrderByDescending(i => i.Number).ToArray();

            var arcs = this.arcsRepository
                .All()
                .Where(a => a.ArcsVolumes.Any(av => av.VolumeId == currentVolume.Id))
                .Select(a => new EntityLinkingModel
                {
                    Id = a.Id,
                    CoverPath = a.CoverPath,
                    Title = a.Title,
                    Number = a.Number,
                }).OrderByDescending(a => a.Number).ToArray();

            currentVolume.Issues = issues;
            currentVolume.Arcs = arcs;

            return currentVolume;
        }
    }
}