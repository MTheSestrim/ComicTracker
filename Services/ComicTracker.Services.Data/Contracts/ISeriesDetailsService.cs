namespace ComicTracker.Services.Data.Contracts
{
    using ComicTracker.Web.ViewModels.Series;

    public interface ISeriesDetailsService
    {
        SeriesDetailsViewModel GetSeries(int seriesId);
    }
}
