namespace ComicTracker.Services.Data.Contracts
{
    using System.Collections.Generic;

    using ComicTracker.Web.ViewModels.Home;

    public interface IHomePageService
    {
        IEnumerable<HomeSeriesViewModel> GetSeries();
    }
}
