namespace ComicTracker.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Web.ViewModels.Issues;
    using Microsoft.EntityFrameworkCore;

    public class IssueDetailsService : IIssueDetailsService
    {
        private readonly IDeletableEntityRepository<Issue> issuesRepository;

        public IssueDetailsService(IDeletableEntityRepository<Issue> issuesRepository)
        {
            this.issuesRepository = issuesRepository;
        }

        public async Task<IssueDetailsViewModel> GetIssueAsync(int issueId)
        {
            var currentIssue = await this.issuesRepository
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
                   ArcNumber = i.Arc.Number,
                   ArcTitle = i.Arc.Title,
                   VolumeId = i.VolumeId,
                   VolumeNumber = i.Volume.Number,
                   VolumeTitle = i.Volume.Title,
               })
               .FirstOrDefaultAsync(i => i.Id == issueId);

            if (currentIssue == null)
            {
                return null;
            }

            return currentIssue;
        }
    }
}
