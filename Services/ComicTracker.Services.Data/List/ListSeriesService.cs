namespace ComicTracker.Services.Data.List
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Data.Models.Entities;
    using ComicTracker.Services.Data.List.Contracts;

    using Microsoft.EntityFrameworkCore;

    public class ListSeriesService : IListSeriesService
    {
        private readonly ComicTrackerDbContext dbContext;

        public ListSeriesService(ComicTrackerDbContext dbContext)
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

            var user = this.dbContext.Users
                .Include(u => u.UsersSeries)
                .FirstOrDefault(u => u.Id == userId);

            if (user.UsersSeries.Any(uv => uv.Series == series))
            {
                throw new InvalidOperationException("User has already added given series to their list.");
            }

            var userSeries = new UserSeries { User = user, Series = series };

            user.UsersSeries.Add(userSeries);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task RemoveSeriesFromList(string userId, int id)
        {
            var series = this.dbContext.Series.Find(id);

            if (series == null)
            {
                throw new KeyNotFoundException("Series with given id does not exist.");
            }

            var user = this.dbContext.Users
                .Include(u => u.UsersSeries)
                .FirstOrDefault(u => u.Id == userId);

            var userSeries = user.UsersSeries.FirstOrDefault(uv => uv.Series == series);

            if (userSeries == null)
            {
                throw new InvalidOperationException("User does not have given series in their list.");
            }

            user.UsersSeries.Remove(userSeries);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
