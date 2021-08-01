namespace ComicTracker.Services.Data.Contracts
{
    using ComicTracker.Services.Data.Models.Series;

    public interface ISeriesDetailsService
    {
        SeriesDetailsServiceModel GetSeries(int seriesId);
    }
}
