namespace ComicTracker.Services.Data.Series.Contracts
{
    using System.Threading.Tasks;

    public interface ISeriesDeletionService
    {
        Task<bool> DeleteSeries(int seriesId);
    }
}
