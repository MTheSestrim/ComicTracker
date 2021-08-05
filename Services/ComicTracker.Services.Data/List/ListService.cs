namespace ComicTracker.Services.Data.List
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using ComicTracker.Data;
    using ComicTracker.Services.Data.List.Contracts;
    using ComicTracker.Services.Data.List.Models;
    using Microsoft.EntityFrameworkCore;

    public class ListService : IListService
    {
        private readonly ComicTrackerDbContext dbContext;
        private readonly IMapper mapper;

        public ListService(ComicTrackerDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public IEnumerable<ListServiceModel> GetListData(string userId) => this.dbContext.Series
               .AsNoTracking()
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
