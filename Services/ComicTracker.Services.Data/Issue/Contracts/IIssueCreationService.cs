namespace ComicTracker.Services.Data.Issue.Contracts
{
    using ComicTracker.Services.Data.Models.Entities;

    public interface IIssueCreationService
    {
        int CreateIssue(CreateSeriesRelatedEntityServiceModel model);
    }
}
