namespace ComicTracker.Services.Data.Issue.Contracts
{
    public interface IIssueDeletionService
    {
        int? DeleteIssue(int issueId);
    }
}
