namespace ComicTracker.Tests.Services.Series
{
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.Models.Entities;
    using ComicTracker.Services.Data.Series.Contracts;

    public class MockSeriesRatingService : ISeriesRatingService
    {
        private readonly ComicTrackerDbContext dbContext;

        public MockSeriesRatingService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int?> RateSeries(string userId, RateApiRequestModel model)
        {
            var series = this.dbContext.Series.Find(model.Id);

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
