namespace ComicTracker.Services.Data.Arc.Contracts
{
    using ComicTracker.Services.Data.Models.Entities;

    public interface IArcEditingService
    {
        int EditArc(EditSeriesRelatedEntityServiceModel model);
    }
}
