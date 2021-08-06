namespace ComicTracker.Services.Data.Volume.Contracts
{
    using ComicTracker.Services.Data.Models.Entities;

    public interface IVolumeEditingService
    {
        int EditVolume(EditSeriesRelatedEntityServiceModel model);
    }
}
