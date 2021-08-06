namespace ComicTracker.Services.Data.Issue.Contracts
{
    using ComicTracker.Services.Data.Models.Entities;

    public interface IIssueEditingService
    {
        int EditIssue(EditSeriesRelatedEntityServiceModel model);
    }
}
