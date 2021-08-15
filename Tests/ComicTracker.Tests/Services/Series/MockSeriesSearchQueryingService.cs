namespace ComicTracker.Tests.Services.Series
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using ComicTracker.Common.Enums;
    using ComicTracker.Data;
    using ComicTracker.Services.Data.Models.Home;
    using ComicTracker.Services.Data.Series.Contracts;

    using static ComicTracker.Common.HomeConstants;

    public class MockSeriesSearchQueryingService : ISeriesSearchQueryingService
    {
        private readonly ComicTrackerDbContext dbContext;
        private readonly IMapper mapper;

        public MockSeriesSearchQueryingService(ComicTrackerDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public IList<HomeSeriesServiceModel> GetSeries(int currentPage, string searchTerm, Sorting sorting)
        {
            return this.dbContext.Series
                .ProjectTo<HomeSeriesServiceModel>(this.mapper.ConfigurationProvider)
                .Skip((currentPage - 1) * SeriesPerPage)
                .Take(SeriesPerPage)
                .ToList();
        }

        public int GetTotalSeriesCount(string searchTerm, Sorting sorting) => this.dbContext.Series.Count();
    }
}
