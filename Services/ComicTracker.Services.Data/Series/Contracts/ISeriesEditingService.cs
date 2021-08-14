namespace ComicTracker.Services.Data.Series.Contracts
{
    using ComicTracker.Services.Data.Series.Models;

    public interface ISeriesEditingService
    {
        int? EditSeries(EditSeriesServiceModel model);
    }
}
