namespace ComicTracker.Services.Data.Contracts
{
    using System.Collections.Generic;

    using ComicTracker.Common.Enums;
    using ComicTracker.Web.ViewModels.Home;

    public interface ISeriesRetrievalService
    {
        int GetTotalSeriesCount(string searchTerm, Sorting sorting);

        IList<HomeSeriesViewModel> GetSeries(int currentPage, string searchTerm, Sorting sorting);
    }
}
