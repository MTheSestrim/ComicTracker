namespace ComicTracker.Services.Data.Volume
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Volume.Contracts;

    using Microsoft.EntityFrameworkCore;

    public class VolumeCreationService : IVolumeCreationService
    {
        private readonly ComicTrackerDbContext dbContext;
        private readonly IFileUploadService fileUploadService;

        public VolumeCreationService(ComicTrackerDbContext dbContext, IFileUploadService fileUploadService)
        {
            this.dbContext = dbContext;
            this.fileUploadService = fileUploadService;
        }

        public int CreateVolume(CreateSeriesRelatedEntityServiceModel model)
        {
            var series = this.dbContext.Series
                .Include(s => s.Volumes)
                .FirstOrDefault(s => s.Id == model.SeriesId);

            if (series == null)
            {
                throw new KeyNotFoundException($"Series with given id {model.SeriesId} does not exist");
            }

            if (series.Volumes.Any(v => v.Number == model.Number))
            {
                throw new InvalidOperationException(
                    $"Cannot insert another {typeof(Volume).Name} with the same number");
            }

            var selectedGenres = new List<Genre>();

            if (model.Genres != null)
            {
                selectedGenres = this.dbContext.Genres
                    .ToList()
                    .Where(g => model.Genres.Any(x => x == g.Id))
                    .ToList();
            }

            Volume newVolume = null;

            if (model.CoverImage == null)
            {
                newVolume = new Volume
                {
                    Title = model.Title,
                    Number = model.Number,
                    Description = model.Description,
                    CoverPath = model.CoverPath,
                    SeriesId = model.SeriesId,
                    Genres = selectedGenres,
                };
            }
            else
            {
                var uniqueFileName = this.fileUploadService.GetUploadedFileName(model.CoverImage, model.Title);

                newVolume = new Volume
                {
                    Title = model.Title,
                    Number = model.Number,
                    Description = model.Description,
                    CoverPath = uniqueFileName,
                    SeriesId = model.SeriesId,
                    Genres = selectedGenres,
                };
            }

            this.dbContext.Volumes.Add(newVolume);
            this.dbContext.SaveChanges();

            return newVolume.Id;
        }
    }
}
