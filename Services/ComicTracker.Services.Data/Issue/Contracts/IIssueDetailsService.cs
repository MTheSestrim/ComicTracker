namespace ComicTracker.Services.Data.Issue.Contracts
{
    using ComicTracker.Services.Data.Issues.Models;

    public interface IIssueDetailsService
    {
        IssueDetailsServiceModel GetIssue(int issueId);
    }
}
