namespace ComicTracker.Services.Data.Volume.Contracts
{
    using ComicTracker.Services.Data.Volume.Models;

    public interface IVolumeDetailsService
    {
        VolumeDetailsServiceModel GetVolume(int volumeId, string userId);
    }
}
