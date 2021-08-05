namespace ComicTracker.Services.Data.Series
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Services.Data.Series.Models;

    using Microsoft.EntityFrameworkCore;

    using static ComicTracker.Services.Data.FileUploadLocator;

    public class SeriesEditingService : ISeriesEditingService
    {
        private readonly ComicTrackerDbContext dbContext;

        public SeriesEditingService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> EditSeriesAsync(EditSeriesServiceModel model)
        {
            var selectedGenres = new List<Genre>();

            if (model.Genres != null)
            {
                selectedGenres = this.dbContext.Genres
                    .ToList()
                    .Where(g => model.Genres.Any(x => x == g.Id))
                    .ToList();
            }

            var currentSeries = this.dbContext.Series.Include(s => s.Genres).FirstOrDefault(s => s.Id == model.Id);

            if (currentSeries == null)
            {
                return -1;
            }

            currentSeries.Title = model.Title;
            currentSeries.Description = model.Description;
            currentSeries.Ongoing = model.Ongoing;
            currentSeries.Genres = selectedGenres;

            // else if -> Only updates thumbnail if data is passed.
            if (model.CoverImage != null)
            {
                var uniqueFileName = await GetUploadedFileNameAsync(model.CoverImage);

                // Delete old cover image and replace it with the new one.
                DeleteCover(currentSeries.CoverPath);
                currentSeries.CoverPath = uniqueFileName;
            }
            else if (model.CoverPath != null)
            {
                currentSeries.CoverPath = model.CoverPath;
            }

            this.dbContext.Update(currentSeries);
            await this.dbContext.SaveChangesAsync();

            return currentSeries.Id;
        }
    }
}
