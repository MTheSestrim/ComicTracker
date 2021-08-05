namespace ComicTracker.Services.Data.Arc.Contracts
{
    using ComicTracker.Services.Data.Models.Entities;

    public interface IArcCreationService
    {
        int CreateArc(CreateSeriesRelatedEntityServiceModel model);
    }
}
