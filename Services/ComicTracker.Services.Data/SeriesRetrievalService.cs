namespace ComicTracker.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Common.Enums;
    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Services.Data.Models.Home;

    using static ComicTracker.Common.HomeConstants;

    public class SeriesRetrievalService : ISeriesRetrievalService
    {
        private readonly IDeletableEntityRepository<Series> seriesRepository;

        public SeriesRetrievalService(IDeletableEntityRepository<Series> seriesRepository)
        {
            this.seriesRepository = seriesRepository;
        }

        public IList<HomeSeriesServiceModel> GetSeries(int currentPage, string searchTerm, Sorting sorting)
        {
            var query = this.FormSeriesQuery(searchTerm, sorting);

            var series = query.Select(s => new HomeSeriesServiceModel
            {
                Id = s.Id,
                Name = s.Name,
                CoverPath = s.CoverPath,
                IssuesCount = s.Issues.Count,
            })
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
            var query = this.seriesRepository.All()
                .Where(s => s.Name.Contains(searchTerm));

            query = sorting switch
            {
                Sorting.Issues => query.OrderByDescending(s => s.Issues.Count)
                                       .ThenBy(s => s.Name),
                Sorting.Status => query.OrderByDescending(s => s.Ongoing)
                                       .ThenBy(s => s.Issues.Count)
                                       .ThenBy(s => s.Name),
                Sorting.Name or _ => query.OrderBy(s => s.Name),
            };

            return query;
        }
    }
}
