namespace ComicTracker.Services.Data.Arc.Contracts
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Models.Entities;

    public interface IArcDetachmentService
    {
        Task<int?> DetachArcs(AttachSRERequestModel model);
    }
}
