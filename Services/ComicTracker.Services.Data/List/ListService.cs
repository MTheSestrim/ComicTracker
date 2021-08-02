namespace ComicTracker.Services.Data.List
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.List.Contracts;
    using ComicTracker.Services.Data.List.Models;

    public class ListService : IListService
    {
        private readonly IDeletableEntityRepository<Series> seriesRepository;

        public ListService(IDeletableEntityRepository<Series> seriesRepository)
        {
            this.seriesRepository = seriesRepository;
        }

        public IEnumerable<ListServiceModel> GetListData(string userId) => this.seriesRepository
               .All()
               .Select(s => new ListServiceModel
               {
                   Title = s.Name,
                   CoverPath = s.CoverPath,
                   Score = s.UsersSeries.FirstOrDefault(us => us.UserId == userId).Score,
                   IssueCount = s.Issues.Count,
                   VolumeCount = s.Volumes.Count,
                   ArcCount = s.Arcs.Count,
               })
               .ToList();
    }
}
