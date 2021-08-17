namespace ComicTracker.Tests.Mocks.Services.List
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.List.Contracts;

    public class MockListSeriesService : IListSeriesService
    {
        private readonly ComicTrackerDbContext dbContext;

        public MockListSeriesService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddSeriesToList(string userId, int id)
        {
            var series = this.dbContext.Series.Find(id);

            if (series == null)
            {
                throw new KeyNotFoundException("Series with given id does not exist.");
            }

            if (series.UsersSeries.Any(ua => ua.UserId == userId))
            {
                throw new InvalidOperationException("User has already added given series to their list.");
            }

            var userSeries = new UserSeries { UserId = userId, Series = series };

            series.UsersSeries.Add(userSeries);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task RemoveSeriesFromList(string userId, int id)
        {
            var series = this.dbContext.Series.Find(id);

            if (series == null)
            {
                throw new KeyNotFoundException("Series with given id does not exist.");
            }

            var userSeries = series.UsersSeries.FirstOrDefault(ua => ua.UserId == userId && ua.SeriesId == id);

            if (userSeries == null)
            {
                throw new InvalidOperationException("User does not have given series in their list.");
            }

            series.UsersSeries.Remove(userSeries);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
