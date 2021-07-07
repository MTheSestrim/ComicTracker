namespace ComicTracker.Services.Data.Contracts
{
    using ComicTracker.Web.ViewModels.Issues;

    public interface IIssueDetailsService
    {
        IssueDetailsViewModel GetIssue(int issueId);
    }
}
