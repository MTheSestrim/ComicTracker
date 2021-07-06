namespace ComicTracker.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using ComicTracker.Web.ViewModels.Arc;

    public interface IArcDetailsService
    {
        Task<ArcDetailsViewModel> GetArcAsync(int arcId);
    }
}
