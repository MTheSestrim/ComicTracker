namespace ComicTracker.Web.Tests.Services.List
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data;
    using ComicTracker.Services.Data.List.Contracts;
    using ComicTracker.Services.Data.List.Models;

    public class MockListService : IListService
    {
        private readonly ComicTrackerDbContext dbContext;

        public MockListService(ComicTrackerDbContext dbContext)
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
                   IssuesCount = s.Issues.Count,
                   VolumesCount = s.Volumes.Count,
                   ArcsCount = s.Arcs.Count,
               })
               .ToList();
    }
}
