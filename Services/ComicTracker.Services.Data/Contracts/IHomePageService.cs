namespace ComicTracker.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ComicTracker.Web.ViewModels.Home;

    public interface IHomePageService
    {
        Task<IEnumerable<HomeSeriesViewModel>> GetSeriesAsync();
    }
}
