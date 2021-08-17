namespace ComicTracker.Tests.Mocks.Services.Arc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Models.Entities;

    public class MockArcCreationService : IArcCreationService
    {
        private readonly ComicTrackerDbContext dbContext;

        public MockArcCreationService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public int CreateArc(CreateSeriesRelatedEntityServiceModel model)
        {
            var series = this.dbContext.Series.Find(model.SeriesId);

            if (series == null)
            {
                throw new KeyNotFoundException($"Series with given id {model.SeriesId} does not exist");
            }

            if (series.Arcs.Any(a => a.Number == model.Number))
            {
                throw new InvalidOperationException(
                    $"Cannot insert another {typeof(Arc).Name} with the same number");
            }

            var selectedGenres = new List<Genre>();

            if (model.Genres != null)
            {
                selectedGenres = this.dbContext.Genres
                    .ToList()
                    .Where(g => model.Genres.Any(x => x == g.Id))
                    .ToList();
            }

            var newArc = new Arc
            {
                Title = model.Title,
                Number = model.Number,
                Description = model.Description,
                CoverPath = model.CoverPath,
                SeriesId = model.SeriesId,
                Genres = selectedGenres,
            };

            this.dbContext.Arcs.Add(newArc);
            this.dbContext.SaveChanges();

            return newArc.Id;
        }
    }
}
