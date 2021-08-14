namespace ComicTracker.Services.Data.Series.Contracts
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Models.Entities;

    public interface ISeriesRatingService
    {
        Task<int?> RateSeries(string userId, RateApiRequestModel model);
    }
}
