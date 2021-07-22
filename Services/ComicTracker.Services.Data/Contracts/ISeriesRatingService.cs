namespace ComicTracker.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface ISeriesRatingService
    {
        Task<int> RateSeries(string userId, int seriesId, int score);
    }
}
