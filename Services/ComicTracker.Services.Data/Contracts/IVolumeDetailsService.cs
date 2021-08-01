namespace ComicTracker.Services.Data.Contracts
{
    using ComicTracker.Services.Data.Models.Volume;

    public interface IVolumeDetailsService
    {
        VolumeDetailsServiceModel GetVolume(int volumeId);
    }
}
