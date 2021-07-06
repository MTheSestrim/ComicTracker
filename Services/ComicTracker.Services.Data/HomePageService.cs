namespace ComicTracker.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Web.ViewModels.Home;

    using Microsoft.EntityFrameworkCore;

    public class HomePageService : IHomePageService
    {
        private readonly IDeletableEntityRepository<Series> seriesRepository;

        public HomePageService(IDeletableEntityRepository<Series> seriesRepository)
        {
            this.seriesRepository = seriesRepository;
        }

        public async Task<IEnumerable<HomeSeriesViewModel>> GetSeriesAsync() => await this.seriesRepository
                .All()
                .Select(s => new HomeSeriesViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    CoverPath = s.CoverPath,
                    IssuesCount = s.Issues.Count,
                })
                .ToListAsync();
    }
}
