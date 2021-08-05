namespace ComicTracker.Services.Data.Volume.Contracts
{
    using ComicTracker.Services.Data.Models.Entities;

    public interface IVolumeCreationService
    {
        int CreateVolume(CreateSeriesRelatedEntityServiceModel model);
    }
}
