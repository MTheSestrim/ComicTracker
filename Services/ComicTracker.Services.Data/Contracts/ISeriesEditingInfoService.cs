namespace ComicTracker.Services.Data.Contracts
{
    using ComicTracker.Services.Data.Models.Series;

    public interface ISeriesEditingInfoService
    {
        public EditInfoSeriesServiceModel GetSeries(int seriesId);
    }
}
