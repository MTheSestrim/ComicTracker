namespace ComicTracker.Services.Data.Series.Contracts
{
    using System.Threading.Tasks;

    public interface ISeriesRatingService
    {
        Task<int> RateSeries(string userId, int seriesId, int score);
    }
}
