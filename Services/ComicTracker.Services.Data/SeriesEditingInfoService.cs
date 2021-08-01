namespace ComicTracker.Services.Data
{
    using System.Linq;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;
    using ComicTracker.Services.Data.Models.Series;

    public class SeriesEditingInfoService : ISeriesEditingInfoService
    {
        private readonly IDeletableEntityRepository<Series> seriesRepository;

        public SeriesEditingInfoService(IDeletableEntityRepository<Series> seriesRepository)
        {
            this.seriesRepository = seriesRepository;
        }

        public EditSeriesServiceModel GetSeries(int seriesId)
        {
            var currentSeries = this.seriesRepository
               .All()
               .Select(s => new EditSeriesServiceModel
               {
                   Id = s.Id,
                   Title = s.Name,
                   Ongoing = s.Ongoing,
                   Description = s.Description,
                   Genres = s.Genres.Select(g => g.Id),
               })
               .FirstOrDefault(s => s.Id == seriesId);

            if (currentSeries == null)
            {
                return null;
            }

            return currentSeries;
        }
    }
}
