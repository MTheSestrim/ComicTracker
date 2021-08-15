namespace ComicTracker.Services.Data.Issue
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.Models.Entities;

    using Microsoft.EntityFrameworkCore;

    public class IssueDetachmentService : IIssueDetachmentService
    {
        private readonly ComicTrackerDbContext dbContext;

        public IssueDetachmentService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int?> DetachIssues(AttachSRERequestModel model)
        {
            var issues = this.dbContext.Issues
                .Where(i => i.Number >= model.MinRange
                    && i.Number <= model.MaxRange
                    && i.SeriesId == model.SeriesId)
                .ToList();

            if (issues == null)
            {
                throw new ArgumentOutOfRangeException("Incorrect issue range given.");
            }

            if (model.ParentTypeName == nameof(Arc))
            {
                var arc = this.dbContext.Arcs
                    .Include(a => a.Issues)
                    .FirstOrDefault(a => a.Id == model.ParentId);

                if (arc == null)
                {
                    throw new ArgumentNullException($"Arc with given id {model.ParentId} does not exist.");
                }

                foreach (var issue in issues)
                {
                    arc.Issues.Remove(issue);
                }

                await this.dbContext.SaveChangesAsync();

                return issues.Count;
            }
            else if (model.ParentTypeName == nameof(Volume))
            {
                var volume = this.dbContext.Volumes
                    .Include(v => v.Issues)
                    .FirstOrDefault(v => v.Id == model.ParentId);

                if (volume == null)
                {
                    throw new ArgumentNullException($"Volume with given id {model.ParentId} does not exist.");
                }

                foreach (var issue in issues)
                {
                    volume.Issues.Remove(issue);
                }

                await this.dbContext.SaveChangesAsync();

                return issues.Count;
            }

            return null;
        }

        public async Task<int?> DetachArcFromIssue(int issueId)
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

        public async Task<int?> DetachVolumeFromIssue(int issueId)
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
