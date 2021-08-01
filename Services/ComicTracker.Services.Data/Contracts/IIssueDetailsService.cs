namespace ComicTracker.Services.Data.Contracts
{
    using ComicTracker.Services.Data.Models.Issues;

    public interface IIssueDetailsService
    {
        IssueDetailsServiceModel GetIssue(int issueId);
    }
}
