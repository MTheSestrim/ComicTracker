namespace ComicTracker.Services.Data.List
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data;
    using ComicTracker.Services.Data.List.Contracts;
    using ComicTracker.Services.Data.List.Models;

    using Microsoft.EntityFrameworkCore;

    public class ListDataService : IListDataService
    {
        private readonly ComicTrackerDbContext dbContext;

        public ListDataService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<ListServiceModel> GetListData(string userId) => this.dbContext.Series
               .AsNoTracking()
               .Where(s => s.UsersSeries.Any(us => us.UserId == userId))
               .Select(s => new ListServiceModel
               {
                   Title = s.Title,
                   CoverPath = s.CoverPath,
                   Score = s.UsersSeries.FirstOrDefault(us => us.UserId == userId).Score,
                   UserIssuesCount = s.Issues.Where(i => i.UsersIssues.Any(ui => ui.UserId == userId)).Count(),
                   UserVolumesCount = s.Volumes.Where(v => v.UsersVolumes.Any(uv => uv.UserId == userId)).Count(),
                   UserArcsCount = s.Arcs.Where(a => a.UsersArcs.Any(ua => ua.UserId == userId)).Count(),
                   TotalIssuesCount = s.Issues.Count,
                   TotalVolumesCount = s.Volumes.Count,
                   TotalArcsCount = s.Arcs.Count,
               })
               .ToList();
    }
}
