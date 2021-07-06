namespace ComicTracker.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using ComicTracker.Web.ViewModels.Volume;

    public interface IVolumeDetailsService
    {
        Task<VolumeDetailsViewModel> GetVolumeAsync(int volumeId);
    }
}
