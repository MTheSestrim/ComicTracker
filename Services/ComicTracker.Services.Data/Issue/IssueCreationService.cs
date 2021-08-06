namespace ComicTracker.Services.Data.Issue
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.Models.Entities;

    using Microsoft.EntityFrameworkCore;

    public class IssueCreationService : IIssueCreationService
    {
        private readonly ComicTrackerDbContext dbContext;
        private readonly IFileUploadService fileUploadService;

        public IssueCreationService(ComicTrackerDbContext dbContext, IFileUploadService fileUploadService)
        {
            this.dbContext = dbContext;
            this.fileUploadService = fileUploadService;
        }

        public int CreateIssue(CreateSeriesRelatedEntityServiceModel model)
        {
            var series = this.dbContext.Series
                .Include(s => s.Issues)
                .FirstOrDefault(s => s.Id == model.SeriesId);

            if (series == null)
            {
                throw new KeyNotFoundException($"Series with given id {model.SeriesId} does not exist");
            }

            if (series.Issues.Any(i => i.Number == model.Number))
            {
                throw new InvalidOperationException(
                    $"Cannot insert another {typeof(Issue).Name} with the same number");
            }

            var selectedGenres = new List<Genre>();

            if (model.Genres != null)
            {
                selectedGenres = this.dbContext.Genres
                    .ToList()
                    .Where(g => model.Genres.Any(x => x == g.Id))
                    .ToList();
            }

            Issue newIssue = null;

            if (model.CoverImage == null)
            {
                newIssue = new Issue
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

                newIssue = new Issue
                {
                    Title = model.Title,
                    Number = model.Number,
                    Description = model.Description,
                    CoverPath = uniqueFileName,
                    SeriesId = model.SeriesId,
                    Genres = selectedGenres,
                };
            }

            this.dbContext.Issues.Add(newIssue);
            this.dbContext.SaveChanges();

            return newIssue.Id;
        }
    }
}
