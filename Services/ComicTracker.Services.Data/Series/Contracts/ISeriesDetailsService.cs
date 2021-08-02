namespace ComicTracker.Services.Data.Series.Contracts
{
    using ComicTracker.Services.Data.Series.Models;

    public interface ISeriesDetailsService
    {
        SeriesDetailsServiceModel GetSeries(int seriesId);
    }
}
