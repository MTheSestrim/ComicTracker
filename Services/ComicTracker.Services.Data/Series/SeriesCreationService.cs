namespace ComicTracker.Services.Data.Series
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Services.Data.Series.Models;

    using static ComicTracker.Services.Data.FileUploadLocator;

    public class SeriesCreationService : ISeriesCreationService
    {
        private readonly ComicTrackerDbContext dbContext;

        public SeriesCreationService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public int CreateSeries(CreateSeriesServiceModel model)
        {
            var selectedGenres = new List<Genre>();

            if (model.Genres != null)
            {
                selectedGenres = this.dbContext.Genres
                    .ToList()
                    .Where(g => model.Genres.Any(x => x == g.Id))
                    .ToList();
            }

            Series newSeries = null;

            if (model.CoverImage == null)
            {
                newSeries = new Series
                {
                    Title = model.Title,
                    Description = model.Description,
                    CoverPath = model.CoverPath,
                    Ongoing = model.Ongoing,
                    Genres = selectedGenres,
                };
            }
            else
            {
                var uniqueFileName = GetUploadedFileName(model.CoverImage, model.Title);

                newSeries = new Series
                {
                    Title = model.Title,
                    Description = model.Description,
                    CoverPath = uniqueFileName,
                    Ongoing = model.Ongoing,
                    Genres = selectedGenres,
                };
            }

            this.dbContext.Series.Add(newSeries);
            this.dbContext.SaveChanges();

            return newSeries.Id;
        }
    }
}
