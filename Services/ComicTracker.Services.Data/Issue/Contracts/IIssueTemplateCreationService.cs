namespace ComicTracker.Services.Data.Issue.Contracts
{
    using ComicTracker.Services.Data.Models.Entities;

    public interface IIssueTemplateCreationService
    {
        int CreateTemplateIssues(TemplateCreateApiRequestModel model);
    }
}
