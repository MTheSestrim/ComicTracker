namespace ComicTracker.Services.Data
{
    using System.Linq;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Contracts;

    public class SeriesRatingService : ISeriesRatingService
    {
        private readonly IDeletableEntityRepository<Series> seriesRepository;

        public SeriesRatingService(IDeletableEntityRepository<Series> seriesRepository)
        {
            this.seriesRepository = seriesRepository;
        }

        public void RateSeries(string userId, int seriesId, int score)
        {
            var series = this.seriesRepository.All().FirstOrDefault(s => s.Id == seriesId);

            series.UsersSeries.FirstOrDefault(us => us.UserId == userId).Score = score;
        }
    }
}
