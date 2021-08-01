namespace ComicTracker.Services.Data.Contracts
{
    using ComicTracker.Services.Data.Models.Arc;

    public interface IArcDetailsService
    {
        ArcDetailsServiceModel GetArc(int arcId);
    }
}
