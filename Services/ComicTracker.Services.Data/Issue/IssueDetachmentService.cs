namespace ComicTracker.Services.Data.Issue
{
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Services.Data.Issue.Contracts;

    public class IssueDetachmentService : IIssueDetachmentService
    {
        private readonly ComicTrackerDbContext dbContext;

        public IssueDetachmentService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int?> DetachArc(int issueId)
        {
            var issue = this.dbContext.Issues.Find(issueId);

            if (issue == null)
            {
                return null;
            }

            issue.ArcId = null;
            await this.dbContext.SaveChangesAsync();

            return issueId;
        }

        public async Task<int?> DetachVolume(int issueId)
        {
            var issue = this.dbContext.Issues.Find(issueId);

            if (issue == null)
            {
                return null;
            }

            issue.VolumeId = null;
            await this.dbContext.SaveChangesAsync();

            return issue.Id;
        }
    }
}
