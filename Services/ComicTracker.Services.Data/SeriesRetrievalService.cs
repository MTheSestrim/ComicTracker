namespace ComicTracker.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Web.ViewModels.Home;

    using static ComicTracker.Common.HomeConstants;

    public class SeriesRetrievalService : ISeriesRetrievalService
    {
        private readonly IDeletableEntityRepository<Series> seriesRepository;

        public SeriesRetrievalService(IDeletableEntityRepository<Series> seriesRepository)
        {
            this.seriesRepository = seriesRepository;
        }

        public IList<HomeSeriesViewModel> GetSeries(int currentPage) => this.seriesRepository
                    .All()
                    .Select(s => new HomeSeriesViewModel
                    {
                        Id = s.Id,
                        Name = s.Name,
                        CoverPath = s.CoverPath,
                        IssuesCount = s.Issues.Count,
                    })
                    .OrderBy(s => s.Name)
                    .Skip((currentPage - 1) * SeriesPerPage)
                    .Take(SeriesPerPage)
                    .ToList();

        public int GetTotalSeriesCount() => this.seriesRepository.All().Count();
    }
}
