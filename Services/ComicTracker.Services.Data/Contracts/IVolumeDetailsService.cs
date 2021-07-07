namespace ComicTracker.Services.Data.Contracts
{
    using ComicTracker.Web.ViewModels.Volume;

    public interface IVolumeDetailsService
    {
        VolumeDetailsViewModel GetVolume(int volumeId);
    }
}
