namespace ComicTracker.Services.Data.Volume
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Volume.Contracts;

    using Microsoft.EntityFrameworkCore;

    using static ComicTracker.Services.Data.FileUploadLocator;

    public class VolumeEditingService : IVolumeEditingService
    {
        private readonly ComicTrackerDbContext dbContext;

        public VolumeEditingService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
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
                return -1;
            }

            currentVolume.Title = model.Title;
            currentVolume.Description = model.Description;
            currentVolume.Number = model.Number;
            currentVolume.Genres = selectedGenres;

            // else if -> Only updates thumbnail if data is passed.
            if (model.CoverImage != null)
            {
                var uniqueFileName = GetUploadedFileName(model.CoverImage, model.Title);

                // Delete old cover image and replace it with the new one.
                DeleteCover(currentVolume.CoverPath);
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
