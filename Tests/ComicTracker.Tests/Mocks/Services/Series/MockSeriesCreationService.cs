namespace ComicTracker.Tests.Mocks.Services.Series
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Services.Data.Series.Models;

    public class MockSeriesCreationService : ISeriesCreationService
    {
        private readonly ComicTrackerDbContext dbContext;

        public MockSeriesCreationService(ComicTrackerDbContext dbContext)
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

            var newSeries = new Series
            {
                Title = model.Title,
                Description = model.Description,
                CoverPath = model.CoverPath,
                Ongoing = model.Ongoing,
                Genres = selectedGenres,
            };

            this.dbContext.Series.Add(newSeries);
            this.dbContext.SaveChanges();

            return newSeries.Id;
        }
    }
}
