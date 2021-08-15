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
                   Artists = i.Artists.Select(a => new NameOnlyLinkingModel { Id = a.Id, Name = a.Name }).ToList(),
                   Writers = i.Writers.Select(w => new NameOnlyLinkingModel { Id = w.Id, Name = w.Name }).ToList(),
                   Characters = i.CharactersIssues
                    .Select(ci => new NameOnlyLinkingModel
                    {
                        Id = ci.Character.Id,
                        Name = ci.Character.Name,
                    }).ToList(),
                   Publishers = i.PublishersIssues
                    .Select(pi => new PublisherLinkingModel
                    {
                        Id = pi.Publisher.Id,
                        Name = pi.Publisher.Name,
                        Nationality = pi.Publisher.Nationality.Name,
                    }).ToList(),
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
