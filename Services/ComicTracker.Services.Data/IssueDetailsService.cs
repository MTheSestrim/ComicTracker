namespace ComicTracker.Services.Data
{
    using System.Linq;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Web.ViewModels.Issues;

    public class IssueDetailsService : IIssueDetailsService
    {
        private readonly IDeletableEntityRepository<Issue> issuesRepository;

        public IssueDetailsService(IDeletableEntityRepository<Issue> issuesRepository)
        {
            this.issuesRepository = issuesRepository;
        }

        public IssueDetailsViewModel GetIssue(int issueId)
        {
            var currentIssue = this.issuesRepository
               .All()
               .Select(i => new IssueDetailsViewModel
               {
                   Id = i.Id,
                   Number = i.Number,
                   Title = i.Title,
                   CoverPath = i.CoverPath,
                   Description = i.Description,
                   SeriesId = i.SeriesId,
                   SeriesTitle = i.Series.Name,
                   ArcId = i.ArcId,
                   ArcTitle = i.Arc.Title,
                   VolumeId = i.VolumeId,
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
