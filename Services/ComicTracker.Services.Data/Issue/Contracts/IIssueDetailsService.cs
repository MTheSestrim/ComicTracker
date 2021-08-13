namespace ComicTracker.Services.Data.Issue.Contracts
{
    using ComicTracker.Services.Data.Issue.Models;

    public interface IIssueDetailsService
    {
        IssueDetailsServiceModel GetIssue(int issueId, string userId);
    }
}
