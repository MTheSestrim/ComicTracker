namespace ComicTracker.Services.Data.Contracts
{
    using System.Collections.Generic;

    using ComicTracker.Common.Enums;
    using ComicTracker.Services.Data.Models.Home;

    public interface ISeriesRetrievalService
    {
        int GetTotalSeriesCount(string searchTerm, Sorting sorting);

        IList<HomeSeriesServiceModel> GetSeries(int currentPage, string searchTerm, Sorting sorting);
    }
}
