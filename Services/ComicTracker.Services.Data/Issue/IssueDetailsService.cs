namespace ComicTracker.Services.Data.Issue
{
    using System.Linq;

    using AutoMapper;

    using ComicTracker.Data;
    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.Issue.Models;
    using ComicTracker.Services.Data.Models.Entities;
    using Microsoft.EntityFrameworkCore;

    public class IssueDetailsService : IIssueDetailsService
    {
        private readonly ComicTrackerDbContext dbContext;

        public IssueDetailsService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IssueDetailsServiceModel GetIssue(int issueId, string userId)
        {
            var currentIssue = this.dbContext.Issues
               .AsNoTracking()
               .Select(i => new IssueDetailsServiceModel
               {
                   Id = i.Id,
                   Number = i.Number,
                   Title = i.Title,
                   CoverPath = i.CoverPath,
                   Description = i.Description,
                   IsInList = i.UsersIssues.Any(ui => ui.UserId == userId),
                   TotalScore = i.UsersIssues.Average(ui => ui.Score).ToString(),
                   UserScore = i.UsersIssues.FirstOrDefault(ui => ui.UserId == userId).Score.ToString(),
                   SeriesId = i.SeriesId,
                   SeriesTitle = i.Series.Title,
                   ArcId = i.ArcId,
                   ArcNumber = i.Arc.Number,
                   ArcTitle = i.Arc.Title,
                   VolumeId = i.VolumeId,
                   VolumeNumber = i.Volume.Number,
                   VolumeTitle = i.Volume.Title,
                   Genres = i.Genres.Select(g => new NameOnlyLinkingModel { Id = g.Id, Name = g.Name }).ToList(),
               })
               .FirstOrDefault(i => i.Id == issueId);

            if (currentIssue == null)
            {
                return null;
            }

            return currentIssue;
        }
    }
}
