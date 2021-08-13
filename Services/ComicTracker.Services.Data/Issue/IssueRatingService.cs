namespace ComicTracker.Services.Data.Issue
{
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.Models.Entities;

    using Microsoft.EntityFrameworkCore;

    public class IssueRatingService : IIssueRatingService
    {
        private readonly ComicTrackerDbContext dbContext;

        public IssueRatingService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> RateIssue(string userId, RateApiRequestModel model)
        {
            var issue = await this.dbContext.Issues
                .Include(i => i.UsersIssues)
                .FirstOrDefaultAsync(i => i.Id == model.Id);

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

            return 0;
        }
    }
}
