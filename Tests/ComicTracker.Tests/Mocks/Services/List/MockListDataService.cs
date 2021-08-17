namespace ComicTracker.Tests.Mocks.Services.List
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data;
    using ComicTracker.Services.Data.List.Contracts;
    using ComicTracker.Services.Data.List.Models;

    public class MockListDataService : IListDataService
    {
        private readonly ComicTrackerDbContext dbContext;

        public MockListDataService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<ListServiceModel> GetListData(string userId) => this.dbContext.Series
               .Where(s => s.UsersSeries.Any(us => us.UserId == userId))
               .Select(s => new ListServiceModel
               {
                   Title = s.Title,
                   CoverPath = s.CoverPath,
                   Score = s.UsersSeries.FirstOrDefault(us => us.UserId == userId).Score,
                   TotalIssuesCount = s.Issues.Count,
                   TotalVolumesCount = s.Volumes.Count,
                   TotalArcsCount = s.Arcs.Count,
               })
               .ToList();
    }
}
