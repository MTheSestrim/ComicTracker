namespace ComicTracker.Web.Tests.Services.Series
{
    using ComicTracker.Data;
    using ComicTracker.Services.Data.Series.Contracts;
    using ComicTracker.Services.Data.Series.Models;

    class MockSeriesDetailsService : ISeriesDetailsService
    {
        private readonly ComicTrackerDbContext dbContext;

        public MockSeriesDetailsService(ComicTrackerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public SeriesDetailsServiceModel GetSeries(int seriesId, string userId)
        {
            var currentSeries = dbContext.Series.Find(seriesId);

            if (currentSeries == null)
            {
                return null;
            }

            var serviceModel = new SeriesDetailsServiceModel { Id = currentSeries.Id };

            return serviceModel;
        }
    }
}
