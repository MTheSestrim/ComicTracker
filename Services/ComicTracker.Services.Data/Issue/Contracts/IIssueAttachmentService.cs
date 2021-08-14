namespace ComicTracker.Services.Data.Issue.Contracts
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Models.Entities;

    public interface IIssueAttachmentService
    {
        Task<int?> AttachIssues(AttachSRERequestModel model);
    }
}
