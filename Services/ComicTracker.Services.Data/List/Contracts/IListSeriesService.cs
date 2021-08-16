namespace ComicTracker.Services.Data.List.Contracts
{
    using System.Threading.Tasks;

    public interface IListSeriesService
    {
        Task AddSeriesToList(string userId, int id);

        Task RemoveSeriesFromList(string userId, int id);
    }
}
