namespace ComicTracker.Services.Data.Issue.Contracts
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Models.Entities;

    public interface IIssueDetachmentService
    {
        /// <summary>
        /// Detaches given issue range from specified series related entity.
        /// </summary>
        /// <param name="model">Model received from AJAX request.</param>
        /// <returns></returns>
        Task<int?> DetachIssues(AttachSRERequestModel model);

        /// <summary>
        /// Detaches the arc connected to given issue if present.
        /// </summary>
        /// <param name="issueId">Id of issue.</param>
        /// <returns></returns>
        Task<int?> DetachArcFromIssue(int issueId);

        /// <summary>
        /// Detaches the volume connected to given issue if present.
        /// </summary>
        /// <param name="issueId">Id of issue.</param>
        /// <returns></returns>
        Task<int?> DetachVolumeFromIssue(int issueId);
    }
}
