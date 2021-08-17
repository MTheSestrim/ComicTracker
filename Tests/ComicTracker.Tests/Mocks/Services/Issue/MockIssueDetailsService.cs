namespace ComicTracker.Tests.Mocks.Services.Issue
{
    using ComicTracker.Data;
    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.Issue.Models;

    public class MockIssueDetailsService : IIssueDetailsService
    {
        private readonly ComicTrackerDbContext dbContext;

        public MockIssueDetailsService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IssueDetailsServiceModel GetIssue(int issueId, string userId)
        {
            var currentIssue = dbContext.Issues.Find(issueId);

            if (currentIssue == null)
            {
                return null;
            }

            var serviceModel = new IssueDetailsServiceModel { Id = currentIssue.Id };

            return serviceModel;
        }
    }
}
