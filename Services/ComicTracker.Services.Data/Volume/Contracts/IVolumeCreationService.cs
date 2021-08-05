namespace ComicTracker.Services.Data.Volume.Contracts
{
    using ComicTracker.Services.Data.Volume.Models;

    public interface IVolumeCreationService
    {
        int CreateVolume(CreateVolumeServiceModel model);
    }
}
