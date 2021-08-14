namespace ComicTracker.Services.Data.Series
{
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Series.Contracts;

    using Microsoft.EntityFrameworkCore;

    public class SeriesRatingService : ISeriesRatingService
    {
        private readonly ComicTrackerDbContext dbContext;

        public SeriesRatingService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int?> RateSeries(string userId, RateApiRequestModel model)
        {
            var series = await this.dbContext.Series
                .Include(s => s.UsersSeries)
                .FirstOrDefaultAsync(s => s.Id == model.Id);

            if (series != null)
            {
                var userSeries = series.UsersSeries.FirstOrDefault(us => us.UserId == userId);

                if (userSeries == null)
                {
                    userSeries = new UserSeries
                    {
                        UserId = userId,
                        SeriesId = model.Id,
                        Score = model.Score,
                    };

                    series.UsersSeries.Add(userSeries);

                    this.dbContext.Update(series);
                    await this.dbContext.SaveChangesAsync();
                }
                else
                {
                    userSeries.Score = model.Score;

                    this.dbContext.Update(series);
                    await this.dbContext.SaveChangesAsync();
                }

                return model.Score;
            }

            return null;
        }
    }
}
