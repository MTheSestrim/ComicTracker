namespace ComicTracker.Services.Data.Arc.Contracts
{
    using ComicTracker.Services.Data.Arc.Models;

    public interface IArcDetailsService
    {
        ArcDetailsServiceModel GetArc(int arcId, string userId);
    }
}
