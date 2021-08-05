namespace ComicTracker.Services.Data.Arc.Contracts
{
    using System.Threading.Tasks;

    public interface IArcRatingService
    {
        Task<int> RateArc(string userId, int arcId, int score);
    }
}
