namespace ComicTracker.Services.Data.Contracts
{
    using System.Collections.Generic;

    using ComicTracker.Common.Enums;
    using ComicTracker.Web.ViewModels.Home;

    public interface ISeriesRetrievalService
    {
        int GetTotalSeriesCount();

        IList<HomeSeriesViewModel> GetSeries(int currentPage, string searchTerm, Sorting sorting);
    }
}
