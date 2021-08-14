namespace ComicTracker.Services.Data.Series
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Services.Data.Series.Models;

    using Microsoft.EntityFrameworkCore;

    public class SeriesEditingService : ISeriesEditingService
    {
        private readonly ComicTrackerDbContext dbContext;
        private readonly IFileUploadService fileUploadService;

        public SeriesEditingService(ComicTrackerDbContext dbContext, IFileUploadService fileUploadService)
        {
            this.dbContext = dbContext;
            this.fileUploadService = fileUploadService;
        }

        public int? EditSeries(EditSeriesServiceModel model)
        {
            var selectedGenres = new List<Genre>();

            if (model.Genres != null)
            {
                selectedGenres = this.dbContext.Genres
                    .ToList()
                    .Where(g => model.Genres.Any(x => x == g.Id))
                    .ToList();
            }

            var currentSeries = this.dbContext.Series
                .Include(s => s.Genres)
                .Where(s => s.Id == model.Id)
                .FirstOrDefault();

            if (currentSeries == null)
            {
                return null;
            }

            currentSeries.Title = model.Title;
            currentSeries.Description = model.Description;
            currentSeries.Ongoing = model.Ongoing;
            currentSeries.Genres = selectedGenres;

            // else if -> Only updates thumbnail if data is passed.
            if (model.CoverImage != null)
            {
                var uniqueFileName = this.fileUploadService.GetUploadedFileName(model.CoverImage, model.Title);

                // Delete old cover image and replace it with the new one.
                this.fileUploadService.DeleteCover(currentSeries.CoverPath);
                currentSeries.CoverPath = uniqueFileName;
            }
            else if (model.CoverPath != null)
            {
                currentSeries.CoverPath = model.CoverPath;
            }

            this.dbContext.Update(currentSeries);
            this.dbContext.SaveChanges();

            return currentSeries.Id;
        }
    }
}
