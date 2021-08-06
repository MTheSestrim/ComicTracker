namespace ComicTracker.Services.Data.Arc.Contracts
{
    using ComicTracker.Services.Data.Models.Entities;

    public interface IArcEditingInfoService
    {
        public EditInfoSeriesRelatedEntityServiceModel GetArc(int arcId);
    }
}
