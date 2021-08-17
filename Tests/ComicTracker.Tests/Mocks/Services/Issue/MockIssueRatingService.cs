namespace ComicTracker.Tests.Mocks.Services.Issue
{
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;

    public class MockIssueRatingService : IIssueRatingService
    {
        private readonly ComicTrackerDbContext dbContext;

        public MockIssueRatingService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int?> RateIssue(string userId, RateApiRequestModel model)
        {
            var issue = this.dbContext.Issues.Find(model.Id);

            if (issue != null)
            {
                var userIssue = issue.UsersIssues.FirstOrDefault(ui => ui.UserId == userId);

                if (userIssue == null)
                {
                    userIssue = new UserIssue
                    {
                        UserId = userId,
                        IssueId = model.Id,
                        Score = model.Score,
                    };

                    issue.UsersIssues.Add(userIssue);

                    this.dbContext.Update(issue);
                    await this.dbContext.SaveChangesAsync();
                }
                else
                {
                    userIssue.Score = model.Score;

                    this.dbContext.Update(issue);
                    await this.dbContext.SaveChangesAsync();
                }

                return model.Score;
            }

            return null;
        }
    }
}
