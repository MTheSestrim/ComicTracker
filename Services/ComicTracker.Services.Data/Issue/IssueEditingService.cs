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

    public class IssueEditingService : IIssueEditingService
    {
        private readonly ComicTrackerDbContext dbContext;
        private readonly IFileUploadService fileUploadService;

        public IssueEditingService(ComicTrackerDbContext dbContext, IFileUploadService fileUploadService)
        {
            this.dbContext = dbContext;
            this.fileUploadService = fileUploadService;
        }

        public int EditIssue(EditSeriesRelatedEntityServiceModel model)
        {
            var selectedGenres = new List<Genre>();

            if (model.Genres != null)
            {
                selectedGenres = this.dbContext.Genres
                    .ToList()
                    .Where(g => model.Genres.Any(x => x == g.Id))
                    .ToList();
            }

            var currentIssue = this.dbContext.Issues.Include(i => i.Genres).FirstOrDefault(i => i.Id == model.Id);

            if (currentIssue == null)
            {
                throw new KeyNotFoundException($"Issue with given id {model.Id} does not exist");
            }

            if (currentIssue.Number != model.Number && this.dbContext.Issues.Any(i => i.Number == model.Number))
            {
                throw new InvalidOperationException(
                    $"Cannot insert another {typeof(Issue).Name} with the same number");
            }

            currentIssue.Title = model.Title;
            currentIssue.Description = model.Description;
            currentIssue.Number = model.Number;
            currentIssue.Genres = selectedGenres;

            // else if -> Only updates thumbnail if data is passed.
            if (model.CoverImage != null)
            {
                var uniqueFileName = this.fileUploadService.GetUploadedFileName(model.CoverImage, model.Title);

                // Delete old cover image and replace it with the new one.
                this.fileUploadService.DeleteCover(currentIssue.CoverPath);
                currentIssue.CoverPath = uniqueFileName;
            }
            else if (model.CoverPath != null)
            {
                currentIssue.CoverPath = model.CoverPath;
            }

            this.dbContext.Update(currentIssue);
            this.dbContext.SaveChanges();

            return currentIssue.Id;
        }
    }
}
