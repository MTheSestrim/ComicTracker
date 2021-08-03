namespace ComicTracker.Services.Data.Series.Contracts
{
    using System.Collections.Generic;

    using ComicTracker.Common.Enums;
    using ComicTracker.Services.Data.Models.Home;

    public interface ISeriesSearchQueryingService
    {
        int GetTotalSeriesCount(string searchTerm, Sorting sorting);

        IList<HomeSeriesServiceModel> GetSeries(int currentPage, string searchTerm, Sorting sorting);
    }
}
