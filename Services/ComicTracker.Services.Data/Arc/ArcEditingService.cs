namespace ComicTracker.Services.Data.Arc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Arc.Contracts;
    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Services.Data.Models.Entities;

    using Microsoft.EntityFrameworkCore;

    public class ArcEditingService : IArcEditingService
    {
        private readonly ComicTrackerDbContext dbContext;
        private readonly IFileUploadService fileUploadService;

        public ArcEditingService(ComicTrackerDbContext dbContext, IFileUploadService fileUploadService)
        {
            this.dbContext = dbContext;
            this.fileUploadService = fileUploadService;
        }

        public int EditArc(EditSeriesRelatedEntityServiceModel model)
        {
            var selectedGenres = new List<Genre>();

            if (model.Genres != null)
            {
                selectedGenres = this.dbContext.Genres
                    .ToList()
                    .Where(g => model.Genres.Any(x => x == g.Id))
                    .ToList();
            }

            var currentArc = this.dbContext.Arcs.Include(a => a.Genres).FirstOrDefault(a => a.Id == model.Id);

            if (currentArc == null)
            {
                throw new KeyNotFoundException($"Arc with given id {model.Id} does not exist");
            }

            if (currentArc.Number != model.Number && this.dbContext.Arcs.Any(a => a.Number == model.Number))
            {
                throw new InvalidOperationException(
                    $"Cannot insert another {typeof(Arc).Name} with the same number");
            }

            currentArc.Title = model.Title;
            currentArc.Description = model.Description;
            currentArc.Number = model.Number;
            currentArc.Genres = selectedGenres;

            // else if -> Only updates thumbnail if data is passed.
            if (model.CoverImage != null)
            {
                var uniqueFileName = this.fileUploadService.GetUploadedFileName(model.CoverImage, model.Title);

                // Delete old cover image and replace it with the new one.
                this.fileUploadService.DeleteCover(currentArc.CoverPath);
                currentArc.CoverPath = uniqueFileName;
            }
            else if (model.CoverPath != null)
            {
                currentArc.CoverPath = model.CoverPath;
            }

            this.dbContext.Update(currentArc);
            this.dbContext.SaveChanges();

            return currentArc.Id;
        }
    }
}
