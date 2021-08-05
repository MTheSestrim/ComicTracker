namespace ComicTracker.Services.Data.Series.Contracts
{
    using ComicTracker.Services.Data.Series.Models;

    public interface ISeriesCreationService
    {
        int CreateSeries(CreateSeriesServiceModel model);
    }
}
