namespace ComicTracker.Services.Data.Series
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using ComicTracker.Common.Enums;
    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Home;
    using ComicTracker.Services.Data.Series.Contracts;

    using Microsoft.EntityFrameworkCore;

    using static ComicTracker.Common.HomeConstants;

    public class SeriesSearchQueryingService : ISeriesSearchQueryingService
    {
        private readonly ComicTrackerDbContext dbContext;
        private readonly IMapper mapper;

        public SeriesSearchQueryingService(ComicTrackerDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public IList<HomeSeriesServiceModel> GetSeries(int currentPage, string searchTerm, Sorting sorting)
        {
            var query = this.FormSeriesQuery(searchTerm, sorting);

            var series = query.ProjectTo<HomeSeriesServiceModel>(this.mapper.ConfigurationProvider)
            .Skip((currentPage - 1) * SeriesPerPage)
            .Take(SeriesPerPage)
            .ToList();

            return series;
        }

        public int GetTotalSeriesCount(string searchTerm, Sorting sorting)
        {
            var query = this.FormSeriesQuery(searchTerm, sorting);
            return query.Count();
        }

        private IQueryable<Series> FormSeriesQuery(string searchTerm, Sorting sorting)
        {
            var query = this.dbContext.Series
                .AsNoTracking()
                .Where(s => s.Title.Contains(searchTerm));

            query = sorting switch
            {
                Sorting.Issues => query.OrderByDescending(s => s.Issues.Count)
                                       .ThenBy(s => s.Title),
                Sorting.Status => query.OrderByDescending(s => s.Ongoing)
                                       .ThenBy(s => s.Issues.Count)
                                       .ThenBy(s => s.Title),
                Sorting.Name or _ => query.OrderBy(s => s.Title),
            };

            return query;
        }
    }
}
