namespace ComicTracker.Services.Data.Contracts
{
    public interface ISeriesRatingService
    {
        void RateSeries(string userId, int seriesId, int score);
    }
}
