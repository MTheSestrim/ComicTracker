namespace ComicTracker.Services.Data.Volume.Contracts
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Models.Entities;

    public interface IVolumeRatingService
    {
        Task<int> RateVolume(string userId, RateApiRequestModel model);
    }
}
