namespace ComicTracker.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface IVolumeRatingService
    {
        Task<int> RateVolume(string userId, int volumeId, int score);
    }
}
