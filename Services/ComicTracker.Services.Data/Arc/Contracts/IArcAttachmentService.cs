namespace ComicTracker.Services.Data.Arc.Contracts
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Models.Entities;

    public interface IArcAttachmentService
    {
        Task<int?> AttachArcs(AttachSRERequestModel model);
    }
}
