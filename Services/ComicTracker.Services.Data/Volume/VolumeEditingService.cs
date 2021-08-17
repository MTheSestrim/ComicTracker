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

    public class VolumeEditingService : IVolumeEditingService
    {
        private readonly ComicTrackerDbContext dbContext;
        private readonly IFileUploadService fileUploadService;

        public VolumeEditingService(ComicTrackerDbContext dbContext, IFileUploadService fileUploadService)
        {
            this.dbContext = dbContext;
            this.fileUploadService = fileUploadService;
        }

        public int EditVolume(EditSeriesRelatedEntityServiceModel model)
        {
            var selectedGenres = new List<Genre>();

            if (model.Genres != null)
            {
                selectedGenres = this.dbContext.Genres
                    .ToList()
                    .Where(g => model.Genres.Any(x => x == g.Id))
                    .ToList();
            }

            var currentVolume = this.dbContext.Volumes.Include(v => v.Genres).FirstOrDefault(v => v.Id == model.Id);

            if (currentVolume == null)
            {
                throw new KeyNotFoundException($"Volume with given id {model.Id} does not exist");
            }

            var series = this.dbContext.Series
                .Select(s => new { s.Id, s.Volumes, })
                .FirstOrDefault(s => s.Id == model.SeriesId);

            if (series == null || currentVolume.SeriesId != series.Id)
            {
                throw new KeyNotFoundException("Wrong series id given for volume.");
            }

            if (currentVolume.Number != model.Number && series.Volumes.Any(v => v.Number == model.Number))
            {
                throw new InvalidOperationException(
                    $"Cannot insert another {typeof(Volume).Name} with the same number");
            }

            currentVolume.Title = model.Title;
            currentVolume.Description = model.Description;
            currentVolume.Number = model.Number;
            currentVolume.Genres = selectedGenres;

            // else if -> Only updates thumbnail if data is passed.
            if (model.CoverImage != null)
            {
                var uniqueFileName = this.fileUploadService.GetUploadedFileName(model.CoverImage, model.Title);

                // Delete old cover image and replace it with the new one.
                this.fileUploadService.DeleteCover(currentVolume.CoverPath);
                currentVolume.CoverPath = uniqueFileName;
            }
            else if (model.CoverPath != null)
            {
                currentVolume.CoverPath = model.CoverPath;
            }

            this.dbContext.Update(currentVolume);
            this.dbContext.SaveChanges();

            return currentVolume.Id;
        }
    }
}
