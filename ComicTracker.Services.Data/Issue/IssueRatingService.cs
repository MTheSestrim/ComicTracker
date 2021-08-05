namespace ComicTracker.Services.Data.Issue
{
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Issue.Contracts;

    using Microsoft.EntityFrameworkCore;

    public class IssueRatingService : IIssueRatingService
    {
        private readonly ComicTrackerDbContext dbContext;

        public IssueRatingService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> RateIssue(string userId, int issueId, int score)
        {
            var issue = await this.dbContext.Issues
                .Include(i => i.UsersIssues)
                .FirstOrDefaultAsync(i => i.Id == issueId);

            if (issue != null)
            {
                var userIssue = issue.UsersIssues.FirstOrDefault(ui => ui.UserId == userId);

                if (userIssue == null)
                {
                    userIssue = new UserIssue
                    {
                        UserId = userId,
                        IssueId = issueId,
                        Score = score,
                    };

                    issue.UsersIssues.Add(userIssue);

                    this.dbContext.Issues.Update(issue);
                    await this.dbContext.SaveChangesAsync();
                }
                else
                {
                    userIssue.Score = score;

                    this.dbContext.Issues.Update(issue);
                    await this.dbContext.SaveChangesAsync();
                }

                return score;
            }

            return 0;
        }
    }
}
