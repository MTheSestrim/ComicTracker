namespace ComicTracker.Services.Data.Volume.Contracts
{
    using ComicTracker.Services.Data.Models.Entities;

    public interface IVolumeEditingInfoService
    {
        public EditInfoSeriesRelatedEntityServiceModel GetVolume(int volumeId);
    }
}
