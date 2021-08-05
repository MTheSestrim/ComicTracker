namespace ComicTracker.Services.Data.Issue
{
    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.Models.Entities;

    public class IssueTemplateCreationService : IIssueTemplateCreationService
    {
        private readonly ComicTrackerDbContext dbContext;

        public IssueTemplateCreationService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public int CreateTemplateIssues(TemplateCreateApiRequestModel model)
        {
            if (this.dbContext.Series.Find(model.SeriesId).Issues.Count > 0)
            {
                return -1;
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
