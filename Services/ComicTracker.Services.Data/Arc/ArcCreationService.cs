namespace ComicTracker.Services.Data.Arc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Models.Entities;

    using Microsoft.EntityFrameworkCore;

    using static ComicTracker.Services.Data.FileUploadLocator;

    public class ArcCreationService : IArcCreationService
    {
        private readonly ComicTrackerDbContext dbContext;

        public ArcCreationService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public int CreateArc(CreateSeriesRelatedEntityServiceModel model)
        {
            var series = this.dbContext.Series
                .Include(s => s.Arcs)
                .FirstOrDefault(s => s.Id == model.SeriesId);

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

            Arc newArc = null;

            if (model.CoverImage == null)
            {
                newArc = new Arc
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
                var uniqueFileName = GetUploadedFileName(model.CoverImage, model.Title);

                newArc = new Arc
                {
                    Title = model.Title,
                    Number = model.Number,
                    Description = model.Description,
                    CoverPath = uniqueFileName,
                    SeriesId = model.SeriesId,
                    Genres = selectedGenres,
                };
            }

            this.dbContext.Arcs.Add(newArc);
            this.dbContext.SaveChanges();

            return newArc.Id;
        }
    }
}
