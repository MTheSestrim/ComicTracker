namespace ComicTracker.Services.Data.Issue.Contracts
{
    public interface IIssueTemplateCreationService
    {
        int CreateIssueTemplates(int numberOfIssues, int seriesId);
    }
}
