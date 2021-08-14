namespace ComicTracker.Services.Data.Volume.Contracts
{
    using System.Threading.Tasks;

    using ComicTracker.Services.Data.Models.Entities;

    public interface IVolumeAttachmentService
    {
        Task<int?> AttachVolumes(AttachSRERequestModel model);
    }
}
