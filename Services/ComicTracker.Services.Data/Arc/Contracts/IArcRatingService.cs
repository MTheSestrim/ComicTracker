namespace ComicTracker.Services.Data.Arc.Contracts
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Models.Entities;

    public interface IArcRatingService
    {
        Task<int> RateArc(string userId, RateApiRequestModel model);
    }
}
