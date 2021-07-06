namespace ComicTracker.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using ComicTracker.Web.ViewModels.Series;

    public interface ISeriesDetailsService
    {
        Task<SeriesDetailsViewModel> GetSeriesAsync(int seriesId);
    }
}
