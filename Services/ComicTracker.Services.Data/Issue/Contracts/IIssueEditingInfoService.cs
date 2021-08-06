namespace ComicTracker.Services.Data.Issue.Contracts
{
    using ComicTracker.Services.Data.Models.Entities;

    public interface IIssueEditingInfoService
    {
        public EditInfoSeriesRelatedEntityServiceModel GetIssue(int issueId);
    }
}
