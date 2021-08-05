namespace ComicTracker.Services.Data.Series
{
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Series.Contracts;

    using Microsoft.EntityFrameworkCore;

    public class SeriesRatingService : ISeriesRatingService
    {
        private readonly ComicTrackerDbContext dbContext;

        public SeriesRatingService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> RateSeries(string userId, int seriesId, int score)
        {
            var series = await this.dbContext.Series
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

                    this.dbContext.Series.Update(series);
                    await this.dbContext.SaveChangesAsync();
                }
                else
                {
                    userSeries.Score = score;

                    this.dbContext.Series.Update(series);
                    await this.dbContext.SaveChangesAsync();
                }

                return score;
            }

            return 0;
        }
    }
}
