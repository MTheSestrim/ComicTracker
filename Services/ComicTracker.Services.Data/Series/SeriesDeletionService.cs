namespace ComicTracker.Services.Data.Series
{
    using ComicTracker.Data;
    using ComicTracker.Services.Data.Series.Contracts;

    public class SeriesDeletionService : ISeriesDeletionService
    {
        private readonly ComicTrackerDbContext dbContext;

        public SeriesDeletionService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool DeleteSeries(int seriesId)
        {
            var series = this.dbContext.Series.Find(seriesId);

            if (series == null)
            {
                return false;
            }

            this.dbContext.Delete(series);
            this.dbContext.SaveChanges();

            return true;
        }
    }
}
