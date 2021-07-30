namespace ComicTracker.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;

    using Microsoft.EntityFrameworkCore;

    public class IssueRatingService : IIssueRatingService
    {
        private readonly IDeletableEntityRepository<Issue> issuesRepository;

        public IssueRatingService(IDeletableEntityRepository<Issue> issuesRepository)
        {
            this.issuesRepository = issuesRepository;
        }

        public async Task<int> RateIssue(string userId, int issueId, int score)
        {
            var issue = await this.issuesRepository.All()
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

                    this.issuesRepository.Update(issue);
                    await this.issuesRepository.SaveChangesAsync();
                }
                else
                {
                    userIssue.Score = score;

                    this.issuesRepository.Update(issue);
                    await this.issuesRepository.SaveChangesAsync();
                }

                return score;
            }

            return 0;
        }
    }
}
