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

    public class IssueAttachmentService : IIssueAttachmentService
    {
        private readonly ComicTrackerDbContext dbContext;

        public IssueAttachmentService(ComicTrackerDbContext context)
        {
            this.dbContext = context;
        }

        public async Task<int?> AttachIssues(AttachSRERequestModel model)
        {
            var issues = this.dbContext.Issues
                .Where(i => i.Number >= model.MinRange
                    && i.Number <= model.MaxRange
                    && i.SeriesId == model.SeriesId)
                .ToList();

            if (issues.Count == 0)
            {
                throw new ArgumentOutOfRangeException();
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
                    arc.Issues.Add(issue);
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
                    volume.Issues.Add(issue);
                }

                await this.dbContext.SaveChangesAsync();

                return issues.Count;
            }

            return null;
        }
    }
}
