namespace ComicTracker.Services.Data.Issue
{
    using ComicTracker.Data;
    using ComicTracker.Services.Data.Issue.Contracts;

    public class IssueDeletionService : IIssueDeletionService
    {
        private readonly ComicTrackerDbContext dbContext;

        public IssueDeletionService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public int DeleteIssue(int issueId)
        {
            var issue = this.dbContext.Issues.Find(issueId);

            if (issue == null)
            {
                return -1;
            }

            var seriesId = issue.SeriesId;

            this.dbContext.Delete(issue);
            this.dbContext.SaveChanges();

            return seriesId;
        }
    }
}
