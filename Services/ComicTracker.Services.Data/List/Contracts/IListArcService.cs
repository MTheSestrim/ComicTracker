namespace ComicTracker.Services.Data.List.Contracts
{
    using System.Threading.Tasks;

    public interface IListArcService
    {
        Task AddArcToList(string userId, int id);

        Task RemoveArcFromList(string userId, int id);
    }
}
