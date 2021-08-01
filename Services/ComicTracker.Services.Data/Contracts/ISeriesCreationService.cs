namespace ComicTracker.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Models.Series;

    public interface ISeriesCreationService
    {
        Task<int> CreateSeriesAsync(CreateSeriesServiceModel model);
    }
}
