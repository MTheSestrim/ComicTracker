namespace ComicTracker.Services.Data.List.Contracts
{
    using System.Threading.Tasks;

    public interface IListIssueService
    {
        Task AddIssueToList(string userId, int id);

        Task RemoveIssueFromList(string userId, int id);
    }
}
