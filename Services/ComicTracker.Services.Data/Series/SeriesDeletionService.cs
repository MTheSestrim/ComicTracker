namespace ComicTracker.Services.Data.Series
{
    using System.Threading.Tasks;

    using ComicTracker.Data;
    using ComicTracker.Services.Data.Series.Contracts;

    using Microsoft.EntityFrameworkCore;

    public class SeriesDeletionService : ISeriesDeletionService
    {
        private readonly ComicTrackerDbContext dbContext;

        public SeriesDeletionService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> DeleteSeries(int seriesId)
        {
            var series = await this.dbContext.Series.FirstOrDefaultAsync(s => s.Id == seriesId);

            if (series == null)
            {
                return false;
            }

            this.dbContext.Delete(series);
            await this.dbContext.SaveChangesAsync();

            return true;
        }
    }
}
