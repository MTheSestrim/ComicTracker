namespace ComicTracker.Services.Data.List
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.List.Contracts;
    using ComicTracker.Services.Data.List.Models;

    public class ListService : IListService
    {
        private readonly IDeletableEntityRepository<Series> seriesRepository;
        private readonly IMapper mapper;

        public ListService(IDeletableEntityRepository<Series> seriesRepository, IMapper mapper)
        {
            this.seriesRepository = seriesRepository;
            this.mapper = mapper;
        }

        public IEnumerable<ListServiceModel> GetListData(string userId) => this.seriesRepository
               .AllAsNoTracking()
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
