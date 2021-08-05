namespace ComicTracker.Services.Data.Issue
{
    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Issue.Contracts;

    public class IssueTemplateCreationService : IIssueTemplateCreationService
    {
        private readonly ComicTrackerDbContext dbContext;

        public IssueTemplateCreationService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public int CreateIssueTemplates(int numberOfIssues, int seriesId)
        {
            var issueTemplates = new Issue[numberOfIssues];

            for (int i = 0; i < numberOfIssues; i++)
            {
                var newIssue = new Issue
                {
                    Number = i + 1,
                    SeriesId = seriesId,
                };

                issueTemplates[i] = newIssue;
            }

            dbContext.Issues.AddRange(issueTemplates);

            return numberOfIssues;
        }
    }
}
