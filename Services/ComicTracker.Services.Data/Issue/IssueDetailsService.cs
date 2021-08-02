namespace ComicTracker.Services.Data.Issue
{
    using System.Linq;
    using System.Security.Claims;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.Issues.Models;

    using Microsoft.AspNetCore.Http;

    public class IssueDetailsService : IIssueDetailsService
    {
        private readonly IDeletableEntityRepository<Issue> issuesRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public IssueDetailsService(
            IDeletableEntityRepository<Issue> issuesRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            this.issuesRepository = issuesRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public IssueDetailsServiceModel GetIssue(int issueId)
        {
            var userId = this.httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var currentIssue = this.issuesRepository
               .All()
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
                   SeriesTitle = i.Series.Name,
                   ArcId = i.ArcId,
                   ArcNumber = i.Arc.Number,
                   ArcTitle = i.Arc.Title,
                   VolumeId = i.VolumeId,
                   VolumeNumber = i.Volume.Number,
                   VolumeTitle = i.Volume.Title,
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
