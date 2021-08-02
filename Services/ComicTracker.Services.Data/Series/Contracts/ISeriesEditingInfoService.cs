namespace ComicTracker.Services.Data.Series.Contracts
{
    using ComicTracker.Services.Data.Series.Models;

    public interface ISeriesEditingInfoService
    {
        public EditInfoSeriesServiceModel GetSeries(int seriesId);
    }
}
