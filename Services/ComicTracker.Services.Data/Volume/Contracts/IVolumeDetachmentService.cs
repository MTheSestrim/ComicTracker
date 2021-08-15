namespace ComicTracker.Services.Data.Volume.Contracts
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Models.Entities;

    public interface IVolumeDetachmentService
    {
        Task<int?> DetachVolumes(AttachSRERequestModel model);
    }
}
