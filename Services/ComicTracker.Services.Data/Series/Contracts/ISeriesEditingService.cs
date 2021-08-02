namespace ComicTracker.Services.Data.Series.Contracts
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Series.Models;

    public interface ISeriesEditingService
    {
        Task<int> EditSeriesAsync(EditSeriesServiceModel model);
    }
}
