namespace ComicTracker.Services.Data.Issue.Contracts
{
    using System.Threading.Tasks;

    public interface IIssueDetachmentService
    {
        Task<int?> DetachArc(int issueId);

        Task<int?> DetachVolume(int issueId);
    }
}
