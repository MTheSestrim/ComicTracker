namespace ComicTracker.Services.Data.Series
{
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data.Common.Repositories;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Series.Contracts;

    using Microsoft.EntityFrameworkCore;

    public class SeriesRatingService : ISeriesRatingService
    {
        private readonly IDeletableEntityRepository<Series> seriesRepository;

        public SeriesRatingService(IDeletableEntityRepository<Series> seriesRepository)
        {
            this.seriesRepository = seriesRepository;
        }

        public async Task<int> RateSeries(string userId, int seriesId, int score)
        {
            var series = await this.seriesRepository.All()
                .Include(s => s.UsersSeries)
                .FirstOrDefaultAsync(s => s.Id == seriesId);

            if (series != null)
            {
                var userSeries = series.UsersSeries.FirstOrDefault(us => us.UserId == userId);

                if (userSeries == null)
                {
                    userSeries = new UserSeries
                    {
                        UserId = userId,
                        SeriesId = seriesId,
                        Score = score,
                    };

                    series.UsersSeries.Add(userSeries);

                    this.seriesRepository.Update(series);
                    await this.seriesRepository.SaveChangesAsync();
                }
                else
                {
                    userSeries.Score = score;

                    this.seriesRepository.Update(series);
                    await this.seriesRepository.SaveChangesAsync();
                }

                return score;
            }

            return 0;
        }
    }
}
