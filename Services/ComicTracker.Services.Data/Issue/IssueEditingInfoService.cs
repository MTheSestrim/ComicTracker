namespace ComicTracker.Services.Data.Issue
{
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using ComicTracker.Data;
    using ComicTracker.Services.Data.Issue.Contracts;
    using ComicTracker.Services.Data.Models.Entities;

    using Microsoft.EntityFrameworkCore;

    public class IssueEditingInfoService : IIssueEditingInfoService
    {
        private readonly ComicTrackerDbContext dbContext;
        private readonly IMapper mapper;

        public IssueEditingInfoService(ComicTrackerDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public EditInfoSeriesRelatedEntityServiceModel GetIssue(int issueId)
        {
            var currentIssue = this.dbContext.Issues
               .AsNoTracking()
               .ProjectTo<EditInfoSeriesRelatedEntityServiceModel>(this.mapper.ConfigurationProvider)
               .FirstOrDefault(i => i.Id == issueId);

            if (currentIssue == null)
            {
                return null;
            }

            return currentIssue;
        }
    }
}
