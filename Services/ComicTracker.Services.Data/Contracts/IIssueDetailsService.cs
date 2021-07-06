namespace ComicTracker.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using ComicTracker.Web.ViewModels.Issues;

    public interface IIssueDetailsService
    {
        Task<IssueDetailsViewModel> GetIssueAsync(int issueId);
    }
}
