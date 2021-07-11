namespace ComicTracker.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface ISeriesDeletionService
    {
        Task<bool> DeleteSeries(int seriesId);
    }
}
