namespace ComicTracker.Services.Data.List.Contracts
{
    using System.Threading.Tasks;

    public interface IListVolumeService
    {
        Task AddVolumeToList(string userId, int id);

        Task RemoveVolumeFromList(string userId, int id);
    }
}
