namespace ComicTracker.Services.Data.Issue
{
    using System.Linq;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.Models.Entities;

    using Microsoft.EntityFrameworkCore;

    public class IssueTemplateCreationService : IIssueTemplateCreationService
    {
        private readonly ComicTrackerDbContext dbContext;

        public IssueTemplateCreationService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public int? CreateTemplateIssues(TemplateCreateApiRequestModel model)
        {
            var issuesWithSeriesId = this.dbContext.Series.Include(s => s.Issues).Select(s => new { s.Id, s.Issues }).FirstOrDefault(s => s.Id == model.SeriesId);

            if (issuesWithSeriesId.Issues.Any() || model.NumberOfEntities < 1)
            {
                return null;
            }

            var templateIssues = new Issue[model.NumberOfEntities];

            for (int i = 0; i < model.NumberOfEntities; i++)
            {
                var templateIssue = new Issue
                {
                    Number = i + 1,
                    SeriesId = model.SeriesId,
                };

                templateIssues[i] = templateIssue;
            }

            this.dbContext.Issues.AddRange(templateIssues);
            this.dbContext.SaveChanges();

            return model.NumberOfEntities;
        }
    }
}
