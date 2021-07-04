namespace ComicTracker.Services.Data.Contracts
{
    using ComicTracker.Web.ViewModels.Arc;

    public interface IArcDetailsService
    {
        ArcDetailsViewModel GetArc(int arcId);
    }
}
