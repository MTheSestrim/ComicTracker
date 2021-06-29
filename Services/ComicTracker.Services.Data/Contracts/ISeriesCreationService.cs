namespace ComicTracker.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using ComicTracker.Web.ViewModels.Series;

    public interface ISeriesCreationService
    {
        Task<int> CreateSeries(CreateSeriesInputModel model);
    }
}
